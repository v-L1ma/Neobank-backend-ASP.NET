namespace Neobank.Models;

public class WithdrawDto
{
    public string CienteId { get; set; } = string.Empty;
    public int Value { get; set; } = 0;
    public string Password { get; set; } = string.Empty;
}