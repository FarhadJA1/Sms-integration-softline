using SoftlineIntegrationSystem.Models.Dtos;

namespace SoftlineIntegrationSystem.Repositories;

public interface IRestaurantRepository
{
    ValueTask<bool> AddAsync(string name);
    ValueTask<bool> UpdateAsync(int id, string name);
    Task<List<RestaurantDto>> GetAllAsync();
    Task<RestaurantDto?> GetByIdAsync(int id);
}
