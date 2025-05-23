using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Neobank.Application.Interfaces.Auth;
using Neobank.Models;
using Neobank.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Neobank.Application.UseCases.Auth;

public class LoginUseCase : ILoginUseCase
{
    private readonly UserManager<Cliente> _userManager;
    private readonly IConfiguration _configuration;

    public LoginUseCase(
        UserManager<Cliente> userManager, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<LoginResponseDto> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.email);

        if (user==null || !await new ValidarSenha(_userManager).Validar(user, dto.password))
        {
            throw new Exception("Senha inválida");
        }
                 
        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
            claims: authClaims,
            signingCredentials: new SigningCredentials( new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)), 
                SecurityAlgorithms.HmacSha256
            )
        );
        
        return new LoginResponseDto{ Cliente = user, Token = token};
    }
}