using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Identity.Entities;
using SoftlineIntegrationSystem.Identity.Helpers;
using SoftlineIntegrationSystem.Identity.Models;
using SoftlineIntegrationSystem.Identity.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoftlineIntegrationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;
    private readonly string secret;

    public UsersController(
        IUserService userService,
        IMapper mapper,
        IConfiguration appSettings)
    {
        _userService = userService;
        _mapper = mapper;
        secret = appSettings["Secret"];
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateModel model)
    {
        User? user = _userService.Authenticate(model.Email, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(secret);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.IsAdmin ? Constants.Admin:Constants.User),
                    new Claim("Email",user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(Constants.TokenLifeTimeInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);
        return Ok(new
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.IsAdmin ? Constants.Admin : Constants.User,
            LastName = user.LastName,
            Token = tokenString
        });
    }
    [HttpPost("create")]
    [Authorize(Roles = Constants.Admin)]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        User user = _mapper.Map<User>(model);

        try
        {
            _userService.Create(user, model.Password!);
            return Ok();
        }
        catch (AppException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = Constants.Admin)]

    public IActionResult GetAll()
    {
        IEnumerable<User> users = _userService.GetAll();
        IList<UserModel> model = _mapper.Map<IList<UserModel>>(users);
        return Ok(model);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Constants.Admin)]

    public IActionResult GetById([FromRoute] int id)
    {
        User? user = _userService.GetById(id);
        if (user is null)
            return BadRequest();
        UserModel model = _mapper.Map<UserModel>(user);
        return Ok(model);
    }
    [HttpPut("update/{id}")]
    [Authorize(Roles = Constants.Admin)]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateModel model)
    {
        User user = _mapper.Map<User>(model);
        user.Id = id;

        try
        {
            _userService.Update(user, model.Password!);
            return Ok();
        }
        catch (AppException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = Constants.Admin)]
    public IActionResult Delete([FromRoute] int id)
    {
        _userService.Delete(id);
        return Ok();
    }

    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetInfo()
    {
        string? data = HttpContext.GetUserEmail();
        if (data is null)
            return BadRequest();

        User? user = _userService.GetByEmail(data);
        if (user is null)
            return BadRequest();

        return Ok(new
        {
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.IsAdmin ? Constants.Admin : Constants.User,
            LastName = user.LastName
        });
    }
}