using Neobank.Application.Interfaces.ChavePix;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.UseCases;

public class EditarChavePixUseCase : IEditarChavePixUseCase
{
    private readonly AppDbContext _context;

    public EditarChavePixUseCase(AppDbContext context){
        _context = context;
    }
    
    public async Task<ChavePix> Editar(EditarChavePixDto dto)
    {
        if(string.IsNullOrEmpty(dto.Id) || string.IsNullOrEmpty(dto.Chave))
        {
            throw new Exception("Todos os campos são obrigatórios.");
        }

        var chavePix = await _context.ChavesPix.FindAsync(dto.Id);

        if (chavePix is null)
        {
            throw new Exception("Nenhuma chave encontrada.");
        }

        chavePix.Chave = dto.Chave;

        await _context.SaveChangesAsync();
        
        return chavePix;
    }
}