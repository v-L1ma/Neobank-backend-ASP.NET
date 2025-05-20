namespace Neobank.Models;

public class TransacaoDto
{
    public string SenderId { get; set; } = string.Empty;
    public string ChavePix { get; set; } = string.Empty;
    public int Value { get; set; } = 0;
    public string Password { get; set; } = string.Empty;
}