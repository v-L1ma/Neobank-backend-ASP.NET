using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neobank.Models;

namespace Neobank.Data;

public class AppDbContext
    : IdentityDbContext<Cliente>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {        
    }

    public DbSet<Transacao> Transacoes { get; set; }
    
    public DbSet<ChavePix> ChavesPix { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Cliente>().ToTable("Clientes");
        builder.Entity<Transacao>().ToTable("Transacoes");
        builder.Entity<ChavePix>().ToTable("ChavesPix");
    }
}

