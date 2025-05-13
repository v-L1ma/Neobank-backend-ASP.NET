namespace Neobank.Models;

public class TransacaoDto
{
    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public int Value { get; set; } = 0;
}