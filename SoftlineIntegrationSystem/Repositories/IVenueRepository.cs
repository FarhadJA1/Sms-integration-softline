using SoftlineIntegrationSystem.Models.Dtos;

namespace SoftlineIntegrationSystem.Repositories;

public interface IVenueRepository
{
    ValueTask<bool> AddAsync(VenueAddDto dto);
    ValueTask<bool> UpdateAsync(VenueUpdateDto dto);
    Task<List<VenueDto>> GetAllAsync();
    Task<List<VenueDto>> GetAllByRestaurantId(int restaurantId);
    Task<VenueDto?> GetByIdAsync(string id);
}
