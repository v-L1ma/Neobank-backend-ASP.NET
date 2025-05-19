using System.ComponentModel.DataAnnotations;

namespace Neobank.Models;

public class ChavePix
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string ClienteId { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Chave { get; set; } = string.Empty;
}