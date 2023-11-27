using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoftlineIntegrationSystem.Models.Dtos;

public class VenueUpdateDto
{
    [Required]
    public string? Id { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    [Required]
    public string NotifiedPersonPhone { get; set; } = default!;
    [Required]
    public string Username { get; set; } = default!;
    [Required]
    public string Apikey { get; set; } = default!;
    [Required]
    public string SenderName { get; set; } = default!;
    [Required]
    public int RestaurantId { get; set; }
    [JsonPropertyName("iikoApiKey")]
    [Required]
    public string IIKOApikey { get; set; } = default!;
    [Required]
    public string HookPassword { get; set; } = default!;
    [Required]
    public string OrganizationId { get; set; } = default!;
}
