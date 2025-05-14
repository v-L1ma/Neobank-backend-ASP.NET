using Microsoft.AspNetCore.Mvc;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WithdrawController (AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
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