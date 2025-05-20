using Neobank.Application.Interfaces.Clientes;
using Neobank.Application.Interfaces.Services;
using Neobank.Data;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.Application.UseCases.Clientes;

public class GetClientInfoByPixUseCase : IGetClientInfoByPixUseCase
{

    private readonly IFindService _findService;
    private readonly AppDbContext _context;

    public GetClientInfoByPixUseCase(IFindService findService, AppDbContext context)
    {
        _findService = findService;
        _context = context;
    }

    public async Task<Cliente> Get(string chavePix)
    {
        var chave = await _findService.FindByChavePix(chavePix);

        if (chave is null)
        {
            throw new Exception("Chave Pix não encontrada");
        }
        
        var clienteInfo = await _context.Users.FindAsync(chave.ClienteId);
        
        if (clienteInfo is null)
        {
            throw new Exception("Nenhum Cliente encontrado.");
        }
        
        return clienteInfo;
    }
}