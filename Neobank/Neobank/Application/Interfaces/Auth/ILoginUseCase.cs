using System.IdentityModel.Tokens.Jwt;
using Neobank.Models;

namespace Neobank.Application.Interfaces.Auth;

public interface ILoginUseCase
{
    Task<LoginResponseDto> Login(LoginDto dto);
}