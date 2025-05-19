using Microsoft.AspNetCore.Identity;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.UseCases;

public class SaqueUseCase : ISaqueUseCase
{
    private readonly AppDbContext _context;
    private readonly UserManager<Cliente> _userManager;

    public SaqueUseCase(AppDbContext context, UserManager<Cliente> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task Sacar(SaqueDto dto)
    {
        if (dto.Value<=0)
        {
            throw new Exception("Valor inválido.");
        }

        var cliente = await _context.Users.FindAsync(dto.ClienteId);

        if (cliente is null)
        {
            throw new Exception("Usuário não encontrado");
        }
        
        if (!await new ValidarSenha(_userManager).Validar(cliente, dto.Password) )
        {
            throw new Exception("Senha inválida.");
        } 

        if (cliente.Balance < dto.Value)
        {
            throw new Exception("Saldo insuficiente");
        }

        cliente.Balance -= dto.Value;
        
        _context.Transacoes.Add(new Transacao
        {
            Data = DateTime.Now,
            ReceiverId = dto.ClienteId,
            SenderId = dto.ClienteId,
            Tipo = "Saque",
            Value = dto.Value
        });
        
        await _context.SaveChangesAsync();
    }
}