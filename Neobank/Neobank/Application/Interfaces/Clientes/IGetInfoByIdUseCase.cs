using Neobank.Models;

namespace Neobank.Application.Interfaces.Clientes;

public interface IGetInfoByIdUseCase
{
    Task<Cliente> Get(string id);
}