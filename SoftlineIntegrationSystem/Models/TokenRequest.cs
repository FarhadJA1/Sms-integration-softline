namespace SoftlineIntegrationSystem.Models;

public class TokenRequest
{
    public string? apiLogin { get; set; }
}
public class TokenResponse
{
    public string? correlationId { get; set; }
    public string? token { get; set; }
}