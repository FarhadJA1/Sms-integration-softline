using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Repositories;

namespace SoftlineIntegrationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VenuesController : ControllerBase
{
    private readonly ILogRepository _logRepository;
    private readonly IVenueRepository _venueRepository;
    private readonly string _appUrl;
    public VenuesController(ILogRepository logRepository, IVenueRepository venueRepository, IConfiguration configuration)
    {
        _logRepository = logRepository;
        _venueRepository = venueRepository;
        _appUrl = configuration["HostFullUrl"];
    }

    [HttpGet]
    public async Task<ActionResult<List<VenueDto>>> GetAll()
    {
        return Ok(await _venueRepository.GetAllAsync());
    }
    [HttpGet("restaurant/{restaurantId:int}")]
    public async Task<ActionResult<List<VenueDto>>> GetAllByRestaurantId([FromRoute] int restaurantId)
    {
        return Ok(await _venueRepository.GetAllByRestaurantId(restaurantId));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<VenueDto>> GetById([FromRoute] string id)
    {
        return Ok(await _venueRepository.GetByIdAsync(id));
    }
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] VenueAddDto venueDto)
    {
        string? email = HttpContext.GetUserEmail();
        bool result = await _venueRepository.AddAsync(venueDto);
        if (result)
        {
            await _logRepository.AddAsync(new Models.Entities.Log
            {
                CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                Description = $"Venue {venueDto.Name} created by {email}",
                Email = email
            });
        }

        return result ? Ok() : BadRequest();
    }
    [HttpPut("update/{id}")]
    public async Task<ActionResult> Update([FromBody] VenueUpdateDto venueDto, string id)
    {
        if (id != venueDto.Id)
            return BadRequest();
        string? email = HttpContext.GetUserEmail();

        bool result = await _venueRepository.UpdateAsync(venueDto);
        if (result)
        {
            await _logRepository.AddAsync(new Models.Entities.Log
            {
                CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                Description = $"Venue {venueDto.Name} updated by {email}",
                Email = email
            });
        }
        return result ? Ok() : BadRequest();
    }

    [HttpGet("hookUrl/{id}")]
    public ActionResult GetHookUrl([FromRoute] string id)
    {
        return Ok(new
        {
            Id = id,
            Url = $"{_appUrl}/sms/{id}",
        });
    }
}
