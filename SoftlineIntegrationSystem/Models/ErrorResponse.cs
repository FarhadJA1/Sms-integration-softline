namespace SoftlineIntegrationSystem.Models;

public class ErrorResponse
{
    public ErrorResponse(int statusCode, string message, string details)
    {
        StatusCode = statusCode;
        Message = message;
        Details = details;
    }

    public int StatusCode { get; set; } = 500;
    public string Message { get; set; } = default!;
    public string Details { get; set; } = default!;
}
