using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Application.Interfaces.ChavePix;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class ChavePixController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICriarChavePixUseCase _criarChavePix;
    private readonly IListarChavesUseCase _listarChavePix;
    private readonly IEditarChavePixUseCase _editarChavePix;
    private readonly IExcluirChavePixUseCase _excluirChavePix;

    public ChavePixController(AppDbContext context, 
        ICriarChavePixUseCase criarChavePix, 
        IListarChavesUseCase listarChaves, 
        IEditarChavePixUseCase editarChavePix, 
        IExcluirChavePixUseCase excluirChavePix)
    {
        _context = context;
        _criarChavePix = criarChavePix;
        _listarChavePix = listarChaves;
        _editarChavePix = editarChavePix;
        _excluirChavePix = excluirChavePix;
    }

    [HttpPost]
    public async Task<IActionResult> CriarChave([FromBody] CriarChavePixDto dto)
    {
        try{
        var novaChave = await _criarChavePix.Criar(dto);        
        return Ok(new{ chavePix = novaChave, msg = "Chave cadastrada com sucesso"});
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    
    }

    [HttpGet("{id}")]
    public IActionResult ListarChaves([FromRoute] string id)
    {
        try{
        var chaves = _listarChavePix.Listar(id);        
        return Ok(new
        {
            chavesPIX = chaves, 
            msg = "Essas são as chaves cadastradas."
        });
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> EditarChave([FromBody] EditarChavePixDto dto)
    {
        try
        {
            var chavePix = await _editarChavePix.Editar(dto);
            return Ok(new {
                msg = "Informações da Chave Pix foram editadas com sucesso",
                chavePixNova = chavePix
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirChave([FromRoute] string id)
    {
        try
        {
            var chavePix = await _excluirChavePix.Excluir(id);
            return Ok(new
            {
                chavePix = chavePix,
                msg = "Chave deletada com sucesso."
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
