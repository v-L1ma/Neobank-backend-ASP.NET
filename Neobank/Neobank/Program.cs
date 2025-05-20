using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Neobank.Application.Interfaces.Services;
using Neobank.Application.UseCases.Clientes;
using Neobank.Data;
using Neobank.Interfaces;
using Neobank.Models;
using Neobank.Services;
using Neobank.UseCases;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITransferenciaUseCase ,TransferenciaUseCase>();
builder.Services.AddScoped<IDepositoUseCase, DepositoUseCase>();
builder.Services.AddScoped<ISaqueUseCase, SaqueUseCase>();
builder.Services.AddScoped<ICobrancaUseCase, CobrancaUseCase>();
builder.Services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();
builder.Services.AddScoped<IGetClienteTransacoesUseCase, GetClienteTransacoesUseCase>();
builder.Services.AddScoped<IGetClientInfoByPixUseCase ,GetClientInfoByPixUseCase>();
builder.Services.AddScoped<IGetInfoByIdUseCase, GetInfoByIdUseCase>();
builder.Services.AddScoped<IFindService, FindService>();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Cliente, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    }).AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();