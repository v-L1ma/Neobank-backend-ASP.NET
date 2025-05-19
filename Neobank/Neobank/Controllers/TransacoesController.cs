using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Models;
using Neobank.Services;
using Neobank.UseCases;

namespace Neobank.Controllers;

[Route("[Controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<Cliente> _userManager;
    private readonly TransferenciaUseCase _transferencia;
    
    public TransacoesController(
        AppDbContext context, 
        UserManager<Cliente> userManager,
        TransferenciaUseCase transferencia
            )
    {
        _context = context;
        _userManager = userManager;
        _transferencia = transferencia;
    }
    
    [HttpPost("Transferir")]
    public async Task<IActionResult> Transferir([FromBody] TransacaoDto dto)
    {

        try
        {
            await _transferencia.Tranferir(dto);
            return Ok("Transferencia realizada com sucesso!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
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
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = dto.ClienteId,
            SenderId = dto.ClienteId,
            Tipo = "Depósito",
            Value = dto.Value
        });
        
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

        var cliente = await _context.Users.FindAsync(dto.ClienteId);

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
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = dto.ClienteId,
            SenderId = dto.ClienteId,
            Tipo = "Saque",
            Value = dto.Value
        });
        
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

        cliente.Balance -= infos.Value;
        receiver.Balance += infos.Value;
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = infos.ReceiverId,
            SenderId = dto.ClientId,
            Tipo = "Pagamento",
            Value = infos.Value
        });

        await _context.SaveChangesAsync();
        
        return Ok("Pagamento feito com sucesso!");
    }
    
    
}