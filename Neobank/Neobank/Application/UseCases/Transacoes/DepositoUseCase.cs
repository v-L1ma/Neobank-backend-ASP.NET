using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;

namespace Neobank.UseCases;

public class DepositoUseCase : IDepositoUseCase
{

    private readonly AppDbContext _context;

    public DepositoUseCase(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<TransferenciaResponseDto> Depositar(DepositDto dto)
    {
        if (dto.Value<=0)
        {
            throw new Exception("O saldo deve ser maior que zero");
        }

        var cliente = await _context.Users.FindAsync(dto.ClienteId);

        if (cliente is null)
        {
            throw new Exception("Por favor, forneça um ID válido");
        }

        cliente.Balance += dto.Value;
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = dto.ClienteId,
            ReceiverName = cliente.Name,
            SenderId = dto.ClienteId,
            SenderName = cliente.Name,
            Tipo = "Depósito",
            Value = dto.Value
        });
        
        await _context.SaveChangesAsync();
        return new TransferenciaResponseDto("Deposito concluído com êxito!");
    }
}