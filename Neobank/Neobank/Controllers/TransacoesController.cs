using Microsoft.AspNetCore.Mvc;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class TransacoesController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost("Transferir")]
    public IActionResult Trasferir([FromBody] TransacaoDto dto)
    {
        var sender = _context.Users.Find(dto.SenderId);
        var receiver = _context.Users.Find(dto.ReceiverId);

        if (sender == null || receiver == null)
        {
            return NotFound();
        }

        if (dto.SenderId == dto.ReceiverId || dto.Value<=0 || sender.Balance < dto.Value)
        {
            return BadRequest();
        }

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

        if (cliente.Balance < dto.Value)
        {
            return BadRequest("Saldo insuficiente");
        }

        cliente.Balance = dto.Value;
        await _context.SaveChangesAsync();

        return Ok("Transferencia concluida com êxito!");

    }
    
    
}