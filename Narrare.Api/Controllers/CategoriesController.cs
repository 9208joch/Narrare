using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using Narrare.Application.DTOs;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public CategoriesController(
    ICategoryService categoryService,
    IUserService userService)
    {
        _categoryService = categoryService;
        _userService = userService;
    }
    private int? GetCurrentUserId()
    {
        if (!Request.Headers.TryGetValue(
                "X-User-Id",
                out var userIdValue))
        {
            return null;
        }

        if (!int.TryParse(userIdValue, out var userId))
        {
            return null;
        }

        return userId;
    }

    private async Task<bool> IsAdmin()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return false;
        }

        var currentUser =
            await _userService.GetByIdAsync(currentUserId.Value);

        if (currentUser == null)
        {
            return false;
        }

        return currentUser.Role ==
               Narrare.Domain.Enums.UserRole.Admin;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDto>>> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();

        var result = categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        var result = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
    [FromBody] Category category)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var createdCategory = await _categoryService.CreateAsync(category);

        var result = new CategoryDto
        {
            Id = createdCategory.Id,
            Name = createdCategory.Name,
            Description = createdCategory.Description
        };

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Update(
    int id,
    Category category)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var updatedCategory =
            await _categoryService.UpdateAsync(id, category);

        if (updatedCategory == null)
        {
            return NotFound();
        }

        var result = new CategoryDto
        {
            Id = updatedCategory.Id,
            Name = updatedCategory.Name,
            Description = updatedCategory.Description
        };

        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}