using Microsoft.AspNetCore.Identity;

namespace Neobank.Models;

public class Cliente : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Birthday { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int Balance { get; set; } = 0;
}