using Neobank.Models;

namespace Neobank.Application.Interfaces.Clientes;

public interface IGetClientInfoByPixUseCase
{
    Task<Cliente> Get(string chavePix);
}