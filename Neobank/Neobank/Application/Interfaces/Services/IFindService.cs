using Neobank.Models;

namespace Neobank.Application.Interfaces.Services;

public interface IFindService
{
    Task<ChavePix> FindByChavePix(string chave);
}