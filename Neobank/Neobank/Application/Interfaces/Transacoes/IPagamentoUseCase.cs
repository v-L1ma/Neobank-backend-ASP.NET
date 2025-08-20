using Neobank.Models;

namespace Neobank.Interfaces;

public interface IPagamentoUseCase
{
    Task<TransferenciaResponseDto> Pagar(PagarDto dto);
}