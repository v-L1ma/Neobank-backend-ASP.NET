using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.UseCases;

public class CobrancaUseCase : ICobrancaUseCase
{
    private readonly AppDbContext _context;

    public CobrancaUseCase(AppDbContext context)
    {
        _context = context;
    }

    public async Task<byte[]> Cobrar(CobrancaDto dto)
    {
        var receiver = await _context.Users.FindAsync(dto.ReceiverId);

        if (receiver is null)
        {
            throw new Exception("Conta não encontrada.");
        }

        if (dto.Value <= 0)
        {
            throw new Exception();
        }

        string? json;

        try
        {
            json = JsonSerializer.Serialize(dto);
        }
        catch (Exception e)
        {
            throw new Exception("Erro ao transformar json em string.");
        }

        var criptografado = Cryptografia.Cryptografar(json);

        if (string.IsNullOrEmpty(criptografado))
        {
            throw new Exception("Erro ao criptografar.");
        }
        
        return QrCodeGenerator.GenerateImage(criptografado);
    }
}