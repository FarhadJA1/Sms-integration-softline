using System.ComponentModel.DataAnnotations;

namespace SoftlineIntegrationSystem.Models.Entities;

public class Restaurant
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    public ICollection<Venue> Venues { get; set; } = default!;
}
