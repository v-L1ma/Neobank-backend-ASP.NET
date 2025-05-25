using System.Security.AccessControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Neobank.Application.Interfaces.Clientes;
using Neobank.Data;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IGetInfoByIdUseCase _infoById;
    private readonly IGetClientInfoByPixUseCase _infoByPix;
    private readonly IGetClienteTransacoesUseCase _getClienteTransacoes;

    public ClienteController(AppDbContext context, IGetInfoByIdUseCase infoById, IGetClientInfoByPixUseCase infoByPix, IGetClienteTransacoesUseCase getClienteTransacoes)
    {
        _context = context;
        _infoById = infoById;
        _infoByPix = infoByPix;
        _getClienteTransacoes = getClienteTransacoes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetClienteInfoById([FromRoute] string id)
    {
        try
        {
            var clienteInfo = await _infoById.Get(id);
            return Ok(clienteInfo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("chavePix/{chavePix}")]
    public async Task<ActionResult> GetClienteInfoByPix([FromRoute] string chavePix)
    {
        try
        {
            var clienteInfo = await _infoByPix.Get(chavePix);
            return Ok(new
            {
                clienteInfo.Id,
                clienteInfo.Name,
                clienteInfo.Email
            });

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("transacoes/{id}")]
    public async Task<IActionResult> GetClienteTransacoes([FromRoute] string id)
    {
        try
        {
            var transacoes = await _getClienteTransacoes.Get(id);
            return Ok(new {transacoes = transacoes, msg = "Transaçoes encontradas."});
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}