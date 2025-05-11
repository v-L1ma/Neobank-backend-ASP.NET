using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neobank.Models;

namespace Neobank.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{ 
        private readonly UserManager<Cliente> _userManager;
        private readonly IConfiguration _configuration;

        public ClienteController(UserManager<Cliente> userManager, IConfiguration configuration)
        {
                _userManager = userManager;
                _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
                var user = new Cliente { Name = model.Name, UserName = model.Email, Email = model.Email, Birthday = model.Birthday};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                        return Created();
                }
                return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
                var user = await _userManager.FindByNameAsync(model.email);

                if (user==null || !await _userManager.CheckPasswordAsync(user, model.password))
                { 
                       return Unauthorized();
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
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });

                
        }
}