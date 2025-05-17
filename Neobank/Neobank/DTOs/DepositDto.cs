namespace Neobank.Models;

public class DepositDto
{
    public String ClienteId { get; set; } = string.Empty;

    public int Value { get; set; } = 0;
}