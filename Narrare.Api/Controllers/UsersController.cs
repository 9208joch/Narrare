using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();

        var result = users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Age = user.Age,
            Location = user.Location,
            Occupation = user.Occupation,
            ProfileImageUrl = user.ProfileImageUrl,
            ShowAge = user.ShowAge,
            ShowLocation = user.ShowLocation,
            ShowOccupation = user.ShowOccupation,
            CreatedAt = user.CreatedAt,
            Role = user.Role
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var result = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Age = user.Age,
            Location = user.Location,
            Occupation = user.Occupation,
            ProfileImageUrl = user.ProfileImageUrl,
            ShowAge = user.ShowAge,
            ShowLocation = user.ShowLocation,
            ShowOccupation = user.ShowOccupation,
            CreatedAt = user.CreatedAt,
            Role = user.Role
        };

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto dto)
    {
        var user = await _userService.RegisterAsync(dto);

        if (user == null)
        {
            return Conflict("Användarnamnet är redan upptaget.");
        }

        var result = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Age = user.Age,
            Location = user.Location,
            Occupation = user.Occupation,
            ProfileImageUrl = user.ProfileImageUrl,
            ShowAge = user.ShowAge,
            ShowLocation = user.ShowLocation,
            ShowOccupation = user.ShowOccupation,
            CreatedAt = user.CreatedAt,
            Role = user.Role
        };

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginUserDto dto)
    {
        var user = await _userService.LoginAsync(dto);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(user);
    }
}