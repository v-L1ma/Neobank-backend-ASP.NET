using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class ChavePixController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICriarChavePixUseCase _criarChavePix;
    private readonly IListarChavesUseCase _listarChaves;

    public ChavePixController(AppDbContext context, ICriarChavePixUseCase criarChavePix , IListarChavesUseCase listarChaves){
        _context = context;
        _criarChavePix = criarChavePix;
        _listarChaves = listarChaves;
    }

    [HttpPost]
    public async Task<IActionResult> CriarChave([FromBody] CriarChavePixDto dto)
    {
        try{
        var novaChave = await _criarChavePix.criar(dto);        
        return Ok(new{ chavePix = novaChave, msg = "Chave cadastrada com sucesso"});
        } catch (Exception e)
        {
            return BadRequest(e.message);
        }
    
    }

    [HttpGet("{id}")]
    public IActionResult ListarChaves([FromRoute] string id)
    {
        try{
        var novaChave = await _listarChaves.Listar(dto);        
        return Ok(new { chavesPIX = chaves, msg = "Essas são as chaves cadastradas."});
        } catch (Exception e)
        {
            return BadRequest(e.message);
        }
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
