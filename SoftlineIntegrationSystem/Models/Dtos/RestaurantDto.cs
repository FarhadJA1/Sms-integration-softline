namespace SoftlineIntegrationSystem.Models.Dtos;

public class RestaurantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<VenueDto> Venues { get; set; } = default!;
}
