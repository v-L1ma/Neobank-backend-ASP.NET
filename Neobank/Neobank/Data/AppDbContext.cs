using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neobank.Models;

namespace Neobank.Data;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<Cliente>(options);

