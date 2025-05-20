using Neobank.Models;

namespace Neobank.Application.Interfaces.ChavePix;

public interface ICriarChavePixUseCase
{
    Task<Models.ChavePix> Criar(CriarChavePixDto dto);
}