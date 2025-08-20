using Neobank.Models;

namespace Neobank.Interfaces;

public interface IDepositoUseCase
{
    Task<TransferenciaResponseDto> Depositar(DepositDto dto);
}