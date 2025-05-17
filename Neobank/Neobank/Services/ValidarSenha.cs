using Microsoft.AspNetCore.Identity;
using Neobank.Models;

namespace Neobank.Services;

public class ValidarSenha (UserManager<Cliente> userManager)
{
    private readonly UserManager<Cliente> _userManager = userManager;

    public async Task<Boolean> Validar(Cliente user ,string senha)
    {
        return await _userManager.CheckPasswordAsync(user, senha);
    }
    
}