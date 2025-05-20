using Microsoft.AspNetCore.Identity;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.UseCases;

public class TransferenciaUseCase : ITransferenciaUseCase
{

    private readonly AppDbContext _context;
    private readonly UserManager<Cliente> _userManager;

    public TransferenciaUseCase(AppDbContext context, UserManager<Cliente> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Transferir(TransacaoDto dto)
    {
        var sender = await _context.Users.FindAsync(dto.SenderId);
        
        var chave = await new FindService(_context).FindByChavePix(dto.ChavePix);

        if (chave is null)
        {
            throw new Exception("Chave PIX inválida.");
        }
        
        var receiver = await _context.Users.FindAsync(chave.ClienteId);

        if (sender == null || receiver == null)
        {
            throw new Exception("Cliente não encontrado.");
        }
        
        if (!await new ValidarSenha(_userManager).Validar(sender, dto.Password) )
        {
            throw new Exception("Senha inválida.");
        } 

        if (dto.SenderId == chave.ClienteId)
        {
            throw new Exception("Não é possivel transferir para si mesmo.");
        }
        
        if (dto.Value<=0)
        {
            throw new Exception("Valor inválido.");
        }
        
        if (sender.Balance < dto.Value)
        {
            throw new Exception("Saldo insuficiente.");
        }

        sender.Balance -= dto.Value;
        receiver.Balance += dto.Value;
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = chave.ClienteId,
            SenderId = dto.SenderId,
            Tipo = "Tranferência",
            Value = dto.Value
        });
        
        await _context.SaveChangesAsync();
    }
    
}