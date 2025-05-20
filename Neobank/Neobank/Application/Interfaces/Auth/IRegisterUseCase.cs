using Neobank.Models;

namespace Neobank.Application.Interfaces.Auth;

public interface IRegisterUseCase
{
    Task<Cliente> Register(RegisterDto dto);
}