using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models.Entities;
using SoftlineIntegrationSystem.Repositories;

namespace SoftlineIntegrationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;

    public LogsController(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Log>>> GetAll([FromQuery] int? pageIndex)
    {
        if (pageIndex is not null)
        {
            List<Log> paginatedLogs = _logRepository.GetPagination(pageIndex.Value, out int count);
            Response.AddTotalCountHeader(count);
            return Ok(paginatedLogs);
        }
        List<Log> data = await _logRepository.GetAllAsync();
        Response.AddTotalCountHeader(data.Count);
        return Ok(data);
    }
    [HttpGet("pagination/{page:int}")]
    public ActionResult<List<Log>> GetPagination([FromRoute] int page)
    {
        List<Log> paginatedLogs = _logRepository.GetPagination(page, out int count);
        Response.AddTotalCountHeader(count);
        return Ok(paginatedLogs);
    }
    [HttpGet("clear")]
    [Authorize(Roles = Constants.Admin)]
    public async Task<ActionResult> ClearDatabase()
    {
        bool result = await _logRepository.ClearDB();
        return result ? Accepted() : BadRequest();
    }
}
