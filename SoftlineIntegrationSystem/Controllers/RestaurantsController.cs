using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Repositories;

namespace SoftlineIntegrationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILogRepository _logRepository;

        public RestaurantsController(IRestaurantRepository restaurantRepository, ILogRepository logRepository)
        {
            _restaurantRepository = restaurantRepository;
            _logRepository = logRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<RestaurantDto>>> GetAll()
        {
            return Ok(await _restaurantRepository.GetAllAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetById([FromRoute] int id)
        {
            return Ok(await _restaurantRepository.GetByIdAsync(id));
        }
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] RestaurantCreateUpdateDto model)
        {
            string? email = HttpContext.GetUserEmail();
            bool result = await _restaurantRepository.AddAsync(model.Name);
            if (result)
            {
                await _logRepository.AddAsync(new Models.Entities.Log
                {
                    CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    Description = $"Restaurant {model.Name} created by {email}",
                    Email = email
                });
            }

            return result ? Ok() : BadRequest();
        }
        [HttpPut("update/{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] RestaurantCreateUpdateDto model)
        {
            string? email = HttpContext.GetUserEmail();

            bool result = await _restaurantRepository.UpdateAsync(id, model.Name);
            if (result)
            {
                await _logRepository.AddAsync(new Models.Entities.Log
                {
                    CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    Description = $"Restaurant {model.Name} updated by {email}",
                    Email = email
                });
            }
            return result ? Ok() : BadRequest();
        }
    }
}
