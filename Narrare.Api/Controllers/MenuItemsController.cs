using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    private readonly IUserService _userService;

    public MenuItemsController(
        IMenuItemService menuItemService,
        IUserService userService)
    {
        _menuItemService = menuItemService;
        _userService = userService;
    }

    private int? GetCurrentUserId()
    {
        if (!Request.Headers.TryGetValue("X-User-Id", out var userIdValue))
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

        return currentUser.Role == Narrare.Domain.Enums.UserRole.Admin;
    }

    // Alla får läsa menyn
    [HttpGet]
    public async Task<ActionResult<List<MenuItemDto>>> GetAll()
    {
        var menuItems = await _menuItemService.GetAllAsync();

        var result = menuItems
            .OrderBy(menuItem => menuItem.SortOrder)
            .Select(menuItem => new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Url = menuItem.Url,
                SortOrder = menuItem.SortOrder
            })
            .ToList();

        return Ok(result);
    }

    // Alla får hämta ett menyalternativ
    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemDto>> GetById(int id)
    {
        var menuItem = await _menuItemService.GetByIdAsync(id);

        if (menuItem == null)
        {
            return NotFound();
        }

        var result = new MenuItemDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Url = menuItem.Url,
            SortOrder = menuItem.SortOrder
        };

        return Ok(result);
    }

    // Endast admin får skapa
    [HttpPost]
    public async Task<ActionResult<MenuItemDto>> Create(
        [FromBody] MenuItem menuItem)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var createdMenuItem =
            await _menuItemService.CreateAsync(menuItem);

        var result = new MenuItemDto
        {
            Id = createdMenuItem.Id,
            Name = createdMenuItem.Name,
            Url = createdMenuItem.Url,
            SortOrder = createdMenuItem.SortOrder
        };

        return Ok(result);
    }

    // Endast admin får ändra
    [HttpPut("{id}")]
    public async Task<ActionResult<MenuItemDto>> Update(
        int id,
        [FromBody] MenuItem menuItem)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var existingMenuItem =
            await _menuItemService.GetByIdAsync(id);

        if (existingMenuItem == null)
        {
            return NotFound();
        }

        menuItem.Id = id;

        var updatedMenuItem =
            await _menuItemService.UpdateAsync(menuItem);

        if (updatedMenuItem == null)
        {
            return NotFound();
        }

        var result = new MenuItemDto
        {
            Id = updatedMenuItem.Id,
            Name = updatedMenuItem.Name,
            Url = updatedMenuItem.Url,
            SortOrder = updatedMenuItem.SortOrder
        };

        return Ok(result);
    }

    // Endast admin får radera
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var deleted = await _menuItemService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}