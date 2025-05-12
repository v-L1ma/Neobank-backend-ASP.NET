using Microsoft.AspNetCore.Mvc;
using Neobank.Data;

namespace Neobank.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class TransacaoController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public IActionResult Trasferir([FromBody] string senderId, string ReceiverId, int value)
    {
        var sender = _context.Users.Find(ReceiverId);
        var receiver = _context.Users.Find(ReceiverId);

        if (sender == null || receiver == null)
        {
            return NotFound();
        }

        if (senderId == ReceiverId || value<=0 || sender.balance < value)
        {
            return BadRequest();
        }

        sender.balance -= value;
        receiver.balance += value;

        return Ok();
    }
    
    
}