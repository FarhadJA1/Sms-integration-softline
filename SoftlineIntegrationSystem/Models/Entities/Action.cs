namespace SoftlineIntegrationSystem.Models.Entities;

public class Action
{
    public int Id { get; set; }
    public string? VenueId { get; set; }
    public string? VenueName { get; set; }
    public string? Body { get; set; }
    public string? Response { get; set; }
    public bool IsNotified { get; set; }
    public string CreatedDate { get; set; } = default!;
}
