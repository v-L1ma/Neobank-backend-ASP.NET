using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class ChavePixController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpPost]
    public async Task<IActionResult> CriarChave([FromBody] CriarChavePixDto dto)
    {
        if (dto.ClienteId.IsNullOrEmpty() || dto.Chave.IsNullOrEmpty() || dto.Tipo.IsNullOrEmpty())
        {
            return BadRequest("Todos os campos são obrigatórios.");
        }

        var cliente = await _context.Users.FindAsync(dto.ClienteId);

        if (cliente is null)
        {
            return NotFound("Conta não encontrada.");
        }

        if (_context.ChavesPix.Any(chave => chave.ClienteId == cliente.Id && chave.Tipo == dto.Tipo ) )
        {
            return BadRequest($"Já existe uma chave do tipo {dto.Tipo}.");
        }

        ChavePix novaChave;

        switch (dto.Tipo)
        {
            case "Celular":
                novaChave = new ChavePix
                {
                    ClienteId = dto.ClienteId,
                    Tipo = "Celular",
                    Chave = dto.Chave
                };
                break;
            case "CPF":
                novaChave = new ChavePix
                {
                    ClienteId = dto.ClienteId,
                    Tipo = "CPF",
                    Chave = dto.Chave
                };
                break;
            case "Email":
                novaChave = new ChavePix
                {
                    ClienteId = dto.ClienteId,
                    Tipo = "Email",
                    Chave = dto.Chave
                };
                break;
            case "Aleatória":
                novaChave = new ChavePix
                {
                    ClienteId = dto.ClienteId,
                    Tipo = "Aleatória",
                    Chave = Guid.NewGuid().ToString()
                };
                break;
            default:
                return BadRequest("Por favor insira um tipo de chave PIX válido.");
        }

        _context.ChavesPix.Add(novaChave);
        await _context.SaveChangesAsync();
        
        return Ok(new{ chavePix = novaChave, msg = "Chave cadastrada com sucesso"});
    }

    [HttpGet("{id}")]
    public IActionResult ListarChaves([FromRoute] string id)
    {
        if (id.IsNullOrEmpty())
        {
            return BadRequest("Por favor, forneça um Id.");
        }

        List<ChavePix> chaves = _context.ChavesPix.Where(chave => chave.ClienteId == id).ToList();

        if (chaves.IsNullOrEmpty())
        {
            return NotFound("Nenhuma chave PIX encontrada.");
        }
        
        return Ok(new { chavesPIX = chaves, msg = "Essas são as chaves cadastradas."});
    }

    [HttpPatch]
    public async Task<IActionResult> EditarChave([FromBody] EditarChavePixDto dto)
    {
        
        if(dto.Id.IsNullOrEmpty() || dto.Chave.IsNullOrEmpty())
        {
            return BadRequest("Todos os campos são obrigatórios.");
        }

        var chavePix = await _context.ChavesPix.FindAsync(dto.Id);

        if (chavePix is null)
        {
            return NotFound("Nenhuma chave encontrada.");
        }

        chavePix.Chave = dto.Chave;

        await _context.SaveChangesAsync();
        
        return Ok(new {
            msg = "Informações da Chave Pix foram editadas com sucesso",
            chavePixNova = chavePix
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirChave([FromRoute] string id)
    {
        if (id.IsNullOrEmpty())
        {
            return BadRequest("Por favor, forneça um Id");
        }

        var chavePix = await _context.ChavesPix.FindAsync(id);

        if (chavePix is null)
        {
            return NotFound("Chave não encontrada.");
        }

        _context.ChavesPix.Remove(chavePix);
        await _context.SaveChangesAsync();
        
        return Ok(new
        {
            chavePix = chavePix,
            msg = "Chave deletada com sucesso."
        });
    }

}