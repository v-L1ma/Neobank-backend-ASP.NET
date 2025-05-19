using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;
using Neobank.UseCases;

namespace Neobank.Controllers;

[Route("[Controller]")]
[ApiController]
public class TransacoesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<Cliente> _userManager;
    private readonly ITransferenciaUseCase _transferencia;
    private readonly IDepositoUseCase _deposito;
    private readonly ISaqueUseCase _saque;
    private readonly ICobrancaUseCase _cobrar;
    private readonly IPagamentoUseCase _pagamento;
    
    public TransacoesController(
        AppDbContext context, 
        UserManager<Cliente> userManager,
        ITransferenciaUseCase transferencia, 
        IDepositoUseCase deposito, 
        ISaqueUseCase saque, 
        ICobrancaUseCase cobrar, 
        IPagamentoUseCase pagamento)
    {
        _context = context;
        _userManager = userManager;
        _transferencia = transferencia;
        _deposito = deposito;
        _saque = saque;
        _cobrar = cobrar;
        _pagamento = pagamento;
    }
    
    [HttpPost("Transferir")]
    public async Task<IActionResult> Transferir([FromBody] TransacaoDto dto)
    {

        try
        {
            await _transferencia.Transferir(dto);
            return Ok("Transferencia realizada com sucesso!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
    }
    
    [HttpPost("Depositar")]
    public async Task<IActionResult> Depositar([FromBody] DepositDto dto)
    {
        try
        {
            await _deposito.Depositar(dto);
            return Ok("Deposito concluído com êxito!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("Sacar")]
    public async Task<IActionResult> Sacar([FromBody] SaqueDto dto)
    {
        try
        {
            await _saque.Sacar(dto);
            return Ok("Transferencia concluida com êxito!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Cobrar")]
    public async Task<IActionResult> GerarQrCodeCobranca([FromBody] CobrancaDto dto)
    {
        try
        {
            var image = await _cobrar.Cobrar(dto);
            return File(image, "image/jpeg");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("Pagar")]
    public async Task<ActionResult> PagarQrCode([FromBody] PagarDto dto)
    {
        try
        {
            await _pagamento.Pagar(dto);
            return Ok("Pagamento feito com sucesso!");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}