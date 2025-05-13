using Microsoft.AspNetCore.Mvc;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("api/[Controller]")]
[ApiController]
public class TransacaoController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
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
    
    
}