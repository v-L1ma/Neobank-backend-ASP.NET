using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.UseCases;

public class PagamentoUseCase : IPagamentoUseCase
{
    private readonly AppDbContext _context;
    private readonly UserManager<Cliente> _userManager;

    public PagamentoUseCase(AppDbContext context, UserManager<Cliente> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Pagar(PagarDto dto)
    {
        string descriptografado = Cryptografia.Descryptografar(dto.CodigoBarras);

        if (string.IsNullOrEmpty(descriptografado))
        {
            throw new Exception("Erro ao descriptografar.");
        }

        CobrancaDto? infos;

        try
        {
            infos = JsonSerializer.Deserialize<CobrancaDto>(descriptografado);
        }
        catch (Exception e)
        {
            throw new Exception("Erro ao transformar em json.");
        }
        
        if (infos == null || infos.Value<=0)
        {
            throw new Exception("Valor inválido.");
        }

        var receiver = await _context.Users.FindAsync(infos.ReceiverId);
        var cliente = await _context.Users.FindAsync(dto.ClientId);

        if (receiver is null || cliente is null)
        {
            throw new Exception("Cliente não encontrado.");
        }

        if (!await new ValidarSenha(_userManager).Validar(cliente, dto.Password) )
        {
            throw new Exception("Senha inválida.");
        }

        cliente.Balance -= infos.Value;
        receiver.Balance += infos.Value;
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = infos.ReceiverId,
            SenderId = dto.ClientId,
            Tipo = "Pagamento",
            Value = infos.Value
        });

        await _context.SaveChangesAsync();

    }
}