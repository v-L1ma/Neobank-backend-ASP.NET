using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class ClienteController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet("{id}")]
    public async Task<ActionResult> GetClienteInfo([FromRoute] string id)
    {
        if (id.IsNullOrEmpty())
        {
            return BadRequest("Por favor forneça um Id.");
        }
        var clienteInfo = await _context.Users.FindAsync(id);

        if (clienteInfo is null)
        {
            return NotFound("Usuario não encontrado.");
        }
        
        return Ok(clienteInfo);
    }

    [HttpGet("Transacoes/{id}")]
    public async Task<IActionResult> GetClienteTransacoes([FromRoute] string id)
    {
        if (id.IsNullOrEmpty())
        {
            return BadRequest("Por favor forneça um Id.");
        }

        List<Transacao> transacoes = await _context.Transacoes
            .Where(transacao => transacao.SenderId == id)
            .ToListAsync();

        if (transacoes.IsNullOrEmpty())
        {
            return NotFound("Nenhuma transação encontrada.");
        }

        return Ok(new {transacoes = transacoes, msg = "Transaçoes encontradas."});
    }


}