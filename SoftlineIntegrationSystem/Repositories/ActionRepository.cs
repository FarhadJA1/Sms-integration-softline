using Microsoft.EntityFrameworkCore;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Helpers;

namespace SoftlineIntegrationSystem.Repositories;

public class ActionRepository : IActionRepository
{
    private readonly AppDbContext _context;

    public ActionRepository(AppDbContext context)
    {
        _context = context;
    }
    public async ValueTask<bool> AddAsync(Models.Entities.Action action)
    {
        _context.Actions.Add(action);
        return await _context.SaveChangesAsync() > 0;
    }

    public async ValueTask<bool> ClearDB()
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Actions");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Models.Entities.Action>> GetAllAsync()
    {
        return await _context.Actions.AsNoTracking().OrderByDescending(x=>x.Id).ToListAsync();
    }

    public List<Models.Entities.Action> GetPagination(int page, out int count)
    {
        if (page <= 0)
            page = 1;
        count = _context.Actions.Count();

        IQueryable<Models.Entities.Action> actions = _context.Actions
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * Constants.ItemsPerPage)
            .Take(Constants.ItemsPerPage);

        return actions.ToList();
    }

    public List<Models.Entities.Action> GetPaginationByVenue(int page, string venueId, out int count)
    {
        if (page <= 0)
            page = 1;
        count = _context.Actions.Where(x => x.VenueId == venueId).Count();

        IQueryable<Models.Entities.Action> actions = _context.Actions
            .AsNoTracking()
            .Where(x => x.VenueId == venueId)
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * Constants.ItemsPerPage)
            .Take(Constants.ItemsPerPage);

        return actions.ToList();
    }
}