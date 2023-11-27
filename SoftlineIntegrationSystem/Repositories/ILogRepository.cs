using SoftlineIntegrationSystem.Models.Entities;

namespace SoftlineIntegrationSystem.Repositories;

public interface ILogRepository
{
    ValueTask<bool> AddAsync(Log log);
    Task<List<Log>> GetAllAsync();
    List<Log> GetPagination(int page, out int count);
    List<Log> GetPaginationByVenue(int page, string venueId, out int count);
    ValueTask<bool> ClearDB();
}
