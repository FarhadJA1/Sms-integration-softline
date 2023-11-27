using Microsoft.EntityFrameworkCore;
using SoftlineIntegrationSystem.Data;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Repositories;

public class LogRepository : ILogRepository
{
    private readonly AppDbContext _context;

    public LogRepository(AppDbContext context)
    {
        _context = context;
    }
    public async ValueTask<bool> AddAsync(Log log)
    {
        _context.Logs.Add(log);
        return await _context.SaveChangesAsync() > 0;
    }

    public async ValueTask<bool> ClearDB()
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Logs");
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Log>> GetAllAsync()
    {
        return await _context.Logs.AsNoTracking().OrderByDescending(x=>x.Id).ToListAsync();
    }

    public List<Log> GetPagination(int page, out int count)
    {
        if (page <= 0)
            page = 1;
        count = _context.Logs.Count();

        IQueryable<Log> logs = _context.Logs
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * Constants.ItemsPerPage)
            .Take(Constants.ItemsPerPage);

        return logs.ToList();
    }

    public List<Log> GetPaginationByVenue(int page, string venueId, out int count)
    {
        if (page <= 0)
            page = 1;
        count = _context.Logs.Where(x => x.VenueId == venueId).Count();

        IQueryable<Log> logs = _context.Logs
            .AsNoTracking()
            .Where(x => x.VenueId == venueId)
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * Constants.ItemsPerPage)
            .Take(Constants.ItemsPerPage);

        return logs.ToList();
    }
}