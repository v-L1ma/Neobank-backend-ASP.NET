using Neobank.Models;

namespace Neobank.Interfaces;

public interface IPagamentoUseCase
{
    Task Pagar(PagarDto dto);
}