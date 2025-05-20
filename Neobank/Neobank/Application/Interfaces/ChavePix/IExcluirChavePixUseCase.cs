namespace Neobank.Application.Interfaces.ChavePix;

public interface IExcluirChavePixUseCase
{
    Task<Models.ChavePix> Excluir(string id);
}