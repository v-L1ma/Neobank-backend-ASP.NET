using Neobank.Models;

namespace Neobank.Interfaces;

public interface ITransferenciaUseCase
{
    Task<TransferenciaResponseDto> Transferir(TransacaoDto dto);
}