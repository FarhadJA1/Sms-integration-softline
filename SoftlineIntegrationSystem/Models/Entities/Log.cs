namespace SoftlineIntegrationSystem.Models.Entities;

public class Log
{
    public int Id { get; set; }
    public string? VenueId { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public string? More { get; set; }
    public string CreatedDate { get; set; } = default!;
}
