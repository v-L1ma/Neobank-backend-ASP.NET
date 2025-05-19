using Neobank.Models;

namespace Neobank.Interfaces;

public interface IDepositoUseCase
{
    Task Depositar(DepositDto dto);
}