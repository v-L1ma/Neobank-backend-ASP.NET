using Neobank.Models;

namespace Neobank.Interfaces;

public interface ISaqueUseCase
{
    Task Sacar(SaqueDto dto);
}