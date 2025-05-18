using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;

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
    public IActionResult GetClienteTransacoes([FromRoute] string id)
    {
        if (id.IsNullOrEmpty())
        {
            return BadRequest("Por favor forneça um Id.");
        }

        return Ok("Falta implementar as transacoes");
    }


}