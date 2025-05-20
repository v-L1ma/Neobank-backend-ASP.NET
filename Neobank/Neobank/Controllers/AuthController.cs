using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Application.Interfaces.Auth;
using Neobank.Models;
using Neobank.Services;

namespace Neobank.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{ 
        private readonly ILoginUseCase _login;
        private readonly IRegisterUseCase _register;

        public AuthController(ILoginUseCase login, IRegisterUseCase register)
        {
                _login = login;
                _register = register;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
                try
                {
                        var cliente = await _register.Register(dto);
                        return Ok(new
                        {
                                Cliente = cliente,
                                msg = "Usuario cadastrado com sucesso!"
                        });
                }
                catch (Exception e)
                {
                        return BadRequest(e.Message);
                }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
                try
                {
                        var token = await _login.Login(dto);
                        return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                catch (Exception e)
                {
                        return BadRequest(e.Message);
                }
                
        }
}