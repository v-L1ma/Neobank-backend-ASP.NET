using Neobank.Application.Interfaces.ChavePix;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.UseCases;

public class ListarChavesUseCase : IListarChavesUseCase
{
    private readonly AppDbContext _context;

    public ListarChavesUseCase(AppDbContext context)
    {
        _context = context;
    }

    public List<ChavePix> Listar(string id){

    if (string.IsNullOrEmpty(id))
        {
            throw new Exception("Por favor, forneça um Id.");
        }

        List<ChavePix> chaves = _context.ChavesPix.Where(chave => chave.ClienteId == id).ToList();

        if (chaves.Count == 0)
        {
            throw new Exception("Nenhuma chave PIX encontrada.");
        }

        return chaves;

  }

}
