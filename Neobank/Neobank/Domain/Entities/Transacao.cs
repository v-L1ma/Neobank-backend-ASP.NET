using System.ComponentModel.DataAnnotations;

namespace Neobank.Models;

public class Transacao
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Tipo { get; set; } = string.Empty;
    public string SenderId { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string ReceiverId { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public int Value { get; set; } = 0;
}