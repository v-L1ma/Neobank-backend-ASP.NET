using Microsoft.AspNetCore.Mvc;
using Neobank.Application.Interfaces.Clientes;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Application.UseCases.Clientes;

public class GetInfoByIdUseCase : IGetInfoByIdUseCase
{
    private readonly AppDbContext _context;

    public GetInfoByIdUseCase(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente> Get(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new Exception("Por favor forneça um Id.");
        }
        var clienteInfo = await _context.Users.FindAsync(id);

        if (clienteInfo is null)
        {
            throw new Exception("Usuario não encontrado.");
        }
        
        return clienteInfo;
    }
}