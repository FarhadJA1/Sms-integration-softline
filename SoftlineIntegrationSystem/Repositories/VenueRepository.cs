using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public VenueRepository(IMapper mapper, AppDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }


    public async Task<List<VenueDto>> GetAllAsync()
    {
        List<Venue> venues = await _context.Venues.AsNoTracking().ToListAsync();
        return _mapper.Map<List<VenueDto>>(venues);
    }
    public async Task<List<VenueDto>> GetAllByRestaurantId(int restaurantId)
    {
        List<Venue>? venues = await _context.Venues.AsNoTracking().Where(x => x.RestaurantId == restaurantId).ToListAsync();
        return _mapper.Map<List<VenueDto>>(venues);
    }
    public async Task<VenueDto?> GetByIdAsync(string id)
    {
        Venue? venue = await _context.Venues.FirstOrDefaultAsync(x => x.Id == id);
        if (venue is null)
            return null;
        return _mapper.Map<VenueDto>(venue);
    }

    public async ValueTask<bool> AddAsync(VenueAddDto dto)
    {
        _context.Venues.Add(_mapper.Map<Venue>(dto));
        return await _context.SaveChangesAsync() > 0;
    }

    public async ValueTask<bool> UpdateAsync(VenueUpdateDto dto)
    {
        Venue? updatedVenue = await _context.Venues.FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (updatedVenue is null)
            return false;

        updatedVenue.IsActive = dto.IsActive;
        updatedVenue.RestaurantId = dto.RestaurantId;
        updatedVenue.SenderName = dto.SenderName;
        updatedVenue.Name = dto.Name;
        updatedVenue.Username = dto.Username;
        updatedVenue.Apikey = dto.Apikey;
        updatedVenue.NotifiedPersonPhone = dto.NotifiedPersonPhone;
        updatedVenue.IIKOApikey = dto.IIKOApikey;
        updatedVenue.HookPassword = dto.HookPassword;
        updatedVenue.OrganizationId = dto.OrganizationId;
        return await _context.SaveChangesAsync() > 0;
    }
}