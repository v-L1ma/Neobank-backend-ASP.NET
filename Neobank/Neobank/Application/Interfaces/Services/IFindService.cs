using Neobank.Models;

namespace Neobank.Application.Interfaces.Services;

public interface IFindService
{
    Task<Models.ChavePix> FindByChavePix(string chave);
}