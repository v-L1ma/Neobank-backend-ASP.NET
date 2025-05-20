using Neobank.Models;

namespace Neobank.Application.Interfaces.Clientes;

public interface IGetClienteTransacoesUseCase
{
    Task<List<Transacao>> Get(string id);
}