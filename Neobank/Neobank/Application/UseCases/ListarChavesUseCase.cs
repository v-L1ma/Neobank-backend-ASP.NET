namespace Neobank.UseCases;

public class ListarChavesUseCase : IListarChavesUseCase
{

  public Task<List<ChavePix>> Listar(string id){

    if (id.IsNullOrEmpty())
        {
            throw new Exception("Por favor, forneça um Id.");
        }

        List<ChavePix> chaves = _context.ChavesPix.Where(chave => chave.ClienteId == id).ToList();

        if (chaves.IsNullOrEmpty())
        {
            throw new Exception("Nenhuma chave PIX encontrada.");
        }

        return chaves;

  }

}
