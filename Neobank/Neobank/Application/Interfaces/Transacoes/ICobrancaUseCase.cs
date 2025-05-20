using Neobank.Models;

namespace Neobank.Interfaces;

public interface ICobrancaUseCase
{
    Task<byte[]> Cobrar(CobrancaDto dto);
}