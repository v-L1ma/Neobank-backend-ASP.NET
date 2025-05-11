using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Neobank.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TesteController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("oi");
    }
}