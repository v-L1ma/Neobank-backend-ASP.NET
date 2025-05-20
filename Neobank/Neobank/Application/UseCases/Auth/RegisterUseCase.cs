using Microsoft.AspNetCore.Identity;
using Neobank.Application.Interfaces.Auth;
using Neobank.Models;

namespace Neobank.Application.UseCases.Auth;

public class RegisterUseCase : IRegisterUseCase
{
    private readonly UserManager<Cliente> _userManager;
    private readonly IConfiguration _configuration;

    public RegisterUseCase(UserManager<Cliente> userManager, IConfiguration configuration, ILoginUseCase login)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public async Task<Cliente> Register(RegisterDto dto)
    {
        var user = new Cliente { 
            Name = dto.Name, 
            UserName = dto.Email, 
            Email = dto.Email, 
            Birthday = dto.Birthday,
            Balance = 0
        };
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            return user;
        }
        
        throw new Exception(result.Errors.ToString());
    }
}