using System.Text.Json.Serialization;

namespace SoftlineIntegrationSystem.Models.Dtos;

public class VenueDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public string NotifiedPersonPhone { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Apikey { get; set; } = default!;
    public string SenderName { get; set; } = default!;
    public int RestaurantId { get; set; }
    [JsonPropertyName("iikoApiKey")]
    public string IIKOApikey { get; set; } = default!;
    public string HookPassword { get; set; } = default!;
    public string OrganizationId { get; set; } = default!;
}
