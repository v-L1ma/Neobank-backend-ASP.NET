namespace Neobank.Models;

public class PagarDto
{
    public string ClientId { get; set; } = string.Empty;
    public string CodigoBarras { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}