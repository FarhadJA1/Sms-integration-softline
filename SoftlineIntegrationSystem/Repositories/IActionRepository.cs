namespace SoftlineIntegrationSystem.Repositories;

public interface IActionRepository
{
    ValueTask<bool> AddAsync(Models.Entities.Action action);
    Task<List<Models.Entities.Action>> GetAllAsync();
    List<Models.Entities.Action> GetPagination(int page, out int count);
    List<Models.Entities.Action> GetPaginationByVenue(int page, string venueId, out int count);
    ValueTask<bool> ClearDB();
}
