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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Cliente>().ToTable("Clientes");
    }
}

