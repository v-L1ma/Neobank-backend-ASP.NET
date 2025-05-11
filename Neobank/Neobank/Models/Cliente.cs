using Microsoft.AspNetCore.Identity;

namespace Neobank.Models;

public class Cliente : IdentityUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Birthday { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}