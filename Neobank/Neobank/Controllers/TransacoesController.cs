using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.Controllers;

[Route("[Controller]")]
[ApiController]
public class TransacoesController(AppDbContext context, UserManager<Cliente> userManager) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<Cliente> _userManager = userManager;

    [HttpPost("Transferir")]
    public async Task<IActionResult> Trasferir([FromBody] TransacaoDto dto)
    {
        var sender = await _context.Users.FindAsync(dto.SenderId);
        var receiver = await _context.Users.FindAsync(dto.ReceiverId);

        if (sender == null || receiver == null)
        {
            return NotFound();
        }
        
        if (!await new ValidarSenha(_userManager).Validar(sender, dto.Password) )
        {
            return BadRequest("Senha inválida.");
        } 

        if (dto.SenderId == dto.ReceiverId || dto.Value<=0 || sender.Balance < dto.Value)
        {
            return BadRequest();
        }
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = dto.ReceiverId,
            SenderId = dto.SenderId,
            Tipo = "Tranferência",
            Value = dto.Value
        });

        sender.Balance -= dto.Value;
        receiver.Balance += dto.Value;
        _context.SaveChanges();

        return Ok();
    }
    
    [HttpPost("Depositar")]
    public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
    {
        if (dto.Value<=0)
        {
            return BadRequest("O saldo deve ser maior que zero");
        }

        var cliente = await _context.Users.FindAsync(dto.ClienteId);

        if (cliente is null)
        {
            return BadRequest("Por favor, forneça um ID válido");
        }

        cliente.Balance += dto.Value;
        await _context.SaveChangesAsync();

        return Ok("Deposito concluído com êxito!");
    }
    
    [HttpPost("Sacar")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
    {
        if (dto.Value<=0)
        {
            return BadRequest();
        }

        var cliente = await _context.Users.FindAsync(dto.CienteId);

        if (cliente is null)
        {
            return NotFound();
        }
        
        if (!await new ValidarSenha(_userManager).Validar(cliente, dto.Password) )
        {
            return BadRequest("Senha inválida.");
        } 

        if (cliente.Balance < dto.Value)
        {
            return BadRequest("Saldo insuficiente");
        }

        cliente.Balance = dto.Value;
        await _context.SaveChangesAsync();

        return Ok("Transferencia concluida com êxito!");
    }

    [HttpPost("Cobrar")]
    public async Task<IActionResult> GerarQrCodeCobranca([FromBody] CobrancaDto dto)
    {
        var receiver = await _context.Users.FindAsync(dto.ReceiverId);

        if (receiver is null)
        {
            return NotFound();
        }

        if (dto.Value <= 0)
        {
            return BadRequest();
        }

        string? json;

        try
        {
            json = JsonSerializer.Serialize(dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Erro ao transformar json em string.");
        }

        var criptografado = Cryptografia.Cryptografar(json);

        if (criptografado.IsNullOrEmpty())
        {
            return StatusCode(500, "Erro ao criptografar.");
        }
        
        var image = QrCodeGenerator.GenerateImage(criptografado);
        return File(image, "image/jpeg");
    }

    [HttpPost("Pagar")]
    public async Task<ActionResult> PagarQrCode([FromBody] PagarDto dto)
    {
        string descriptografado = Cryptografia.Descryptografar(dto.CodigoBarras);

        if (descriptografado.IsNullOrEmpty())
        {
            return StatusCode(500, "Erro ao descriptografar.");
        }

        CobrancaDto? infos;

        try
        {
            infos = JsonSerializer.Deserialize<CobrancaDto>(descriptografado);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Erro ao transformar em json.");
        }
        
        if (infos == null || infos.Value<=0)
        {
            return BadRequest();
        }

        var receiver = await _context.Users.FindAsync(infos.ReceiverId);
        var cliente = await _context.Users.FindAsync(dto.ClientId);

        if (receiver is null || cliente is null)
        {
            return NotFound();
        }

        if (!await new ValidarSenha(_userManager).Validar(cliente, dto.Password) )
        {
            return BadRequest("Senha inválida.");
        } 
        
        return Ok(new { receiver = receiver, value = infos.Value});
    }
    
    
}