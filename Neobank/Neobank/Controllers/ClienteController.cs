using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
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
        var clienteInfo = await _context.Users.FindAsync(id);

        return Ok(clienteInfo);
    }


}