using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly AppDbContext _dbContext;
    public IMapper _mapper { get; }

    public RestaurantRepository(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    public async ValueTask<bool> AddAsync(string name)
    {
        _dbContext.Restaurants.Add(new Restaurant
        {
            Name = name
        });
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async ValueTask<bool> UpdateAsync(int id, string name)
    {
        Restaurant? restaurant = await _dbContext.Restaurants.FindAsync(id);
        if (restaurant is null)
            return false;
        restaurant.Name = name;
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<RestaurantDto>> GetAllAsync()
    {
        List<Restaurant> restaurants = await _dbContext.Restaurants.AsNoTracking().Include(x => x.Venues).ToListAsync();
        List<RestaurantDto> restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
        return restaurantsDto;
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id)
    {
        Restaurant? restaurant = await _dbContext.Restaurants.AsNoTracking().Include(x => x.Venues).FirstOrDefaultAsync(x => x.Id == id);
        if (restaurant is null)
            return null;
        return _mapper.Map<RestaurantDto>(restaurant);
    }
}