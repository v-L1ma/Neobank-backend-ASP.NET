using Neobank.Application.Interfaces.ChavePix;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.UseCases;

public class ExcluirChavePixUseCase : IExcluirChavePixUseCase
{
    private readonly AppDbContext _context;

    public ExcluirChavePixUseCase(AppDbContext context){
        _context = context;
    }
    
    public async Task<ChavePix> Excluir(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new Exception("Por favor, forneça um Id");
        }

        var chavePix = await _context.ChavesPix.FindAsync(id);

        if (chavePix is null)
        {
            throw new Exception("Chave não encontrada.");
        }

        _context.ChavesPix.Remove(chavePix);
        await _context.SaveChangesAsync();

        return chavePix;
    }
}