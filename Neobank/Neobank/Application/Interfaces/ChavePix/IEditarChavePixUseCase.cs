using Neobank.Models;

namespace Neobank.Application.Interfaces.ChavePix;

public interface IEditarChavePixUseCase
{
    Task<Models.ChavePix> Editar(EditarChavePixDto dto);
}