using Neobank.Models;

namespace Neobank.Interfaces;

public interface ITransferenciaUseCase
{
    Task Transferir(TransacaoDto dto);
}