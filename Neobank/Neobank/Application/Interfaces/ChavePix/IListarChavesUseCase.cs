namespace Neobank.Application.Interfaces.ChavePix;

public interface IListarChavesUseCase
{
    List<Models.ChavePix> Listar(string id);
}