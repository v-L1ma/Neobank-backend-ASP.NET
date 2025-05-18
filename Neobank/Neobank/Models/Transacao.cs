namespace Neobank.Models;

public class Transacao
{
    public string Id { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public int Value { get; set; } = 0;
}