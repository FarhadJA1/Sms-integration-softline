using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Models.Entities;

public class Venue
{
    public Venue()
    {
        Id = Guid.NewGuid().ToString("N");
    }
    public string Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    public bool IsActive { get; set; }

    [Required]
    public string NotifiedPersonPhone { get; set; } = default!;

    [Required]
    public string IIKOApikey { get; set; } = default!;

    [Required]
    public string HookPassword { get; set; } = default!;

    [Required]
    public string OrganizationId { get; set; } = default!;

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Apikey { get; set; } = default!;

    [Required]
    public string SenderName { get; set; } = default!;

    [Required]
    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = default!;
}