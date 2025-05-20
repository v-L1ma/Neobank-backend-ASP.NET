using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neobank.Application.Interfaces.Services;
using Neobank.Data;
using Neobank.Models;

namespace Neobank.Services;

public class FindService : IFindService
{
    private readonly AppDbContext _context;

    public FindService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ChavePix?> FindByChavePix(string chavePix)
    {
        if (string.IsNullOrEmpty(chavePix))
        {
            return null;
        }

        return await _context.ChavesPix.FirstOrDefaultAsync(chave => chave.Chave == chavePix);
    }
        
}