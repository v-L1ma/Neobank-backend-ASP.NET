using System.IdentityModel.Tokens.Jwt;

namespace Neobank.Models;

public class LoginResponseDto
{
    public JwtSecurityToken Token { get; set; }
    public Cliente Cliente { get; set; }
}