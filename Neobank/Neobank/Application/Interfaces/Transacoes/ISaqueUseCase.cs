using Neobank.Models;

namespace Neobank.Interfaces;

public interface ISaqueUseCase
{
    Task<TransferenciaResponseDto> Sacar(SaqueDto dto);
}