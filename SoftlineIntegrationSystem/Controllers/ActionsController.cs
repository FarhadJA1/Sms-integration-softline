using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models.Entities;
using SoftlineIntegrationSystem.Repositories;

namespace SoftlineIntegrationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ActionsController : ControllerBase
{
    private readonly IActionRepository _actionRepository;

    public ActionsController(IActionRepository actionRepository)
    {
        _actionRepository = actionRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<Log>>> GetAll([FromQuery] int? pageIndex, [FromQuery] string? venueId)
    {
        if(pageIndex is not null && venueId is not null)
        {
            List<Models.Entities.Action> paginatedLogs = _actionRepository.GetPaginationByVenue(pageIndex.Value, venueId, out int count);
            Response.AddTotalCountHeader(count);
            return Ok(paginatedLogs);
        }
        if(pageIndex is not null)
        {
            List<Models.Entities.Action> paginatedLogs = _actionRepository.GetPagination(pageIndex.Value, out int count);
            Response.AddTotalCountHeader(count);
            return Ok(paginatedLogs);
        }
        List<Models.Entities.Action> data = await _actionRepository.GetAllAsync();
        Response.AddTotalCountHeader(data.Count);
        return Ok(data);
    }
    [HttpGet("pagination/{page:int}")]
    public ActionResult<List<Log>> GetPagination([FromRoute] int page)
    {
        List<Models.Entities.Action> paginatedLogs = _actionRepository.GetPagination(page, out int count);
        Response.AddTotalCountHeader(count);
        return Ok(paginatedLogs);
    }
    [HttpGet("pagination/{venueId}/{page:int}")]
    public ActionResult<List<Log>> GetPaginationByVenue([FromRoute] int page, [FromRoute] string venueId)
    {
        List<Models.Entities.Action> paginatedLogs = _actionRepository.GetPaginationByVenue(page, venueId, out int count);
        Response.AddTotalCountHeader(count);
        return Ok(paginatedLogs);
    }
    [HttpGet("clear")]
    [Authorize(Roles = Constants.Admin)]
    public async Task<ActionResult> ClearDatabase()
    {
        bool result = await _actionRepository.ClearDB();
        return result ? Accepted() : BadRequest();
    }
}
