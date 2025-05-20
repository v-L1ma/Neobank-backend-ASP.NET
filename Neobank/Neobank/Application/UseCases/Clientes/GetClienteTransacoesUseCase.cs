using Microsoft.EntityFrameworkCore;
using Neobank.Application.Interfaces.Clientes;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Application.UseCases.Clientes;

public class GetClienteTransacoesUseCase : IGetClienteTransacoesUseCase
{
    private readonly AppDbContext _context;

    public GetClienteTransacoesUseCase(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Transacao>> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new Exception("Por favor forneça um Id.");
        }

        List<Transacao> transacoes = await _context.Transacoes
            .Where(transacao => transacao.SenderId == id)
            .ToListAsync();

        if (transacoes.Count == 0)
        {
            throw new Exception("Nenhuma transação encontrada.");
        }

        return transacoes;
    }
}