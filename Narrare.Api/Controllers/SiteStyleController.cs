using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiteStyleController : ControllerBase
{
    private readonly ISiteStyleService _siteStyleService;
    private readonly IUserService _userService;

    public SiteStyleController(
        ISiteStyleService siteStyleService,
        IUserService userService)
    {
        _siteStyleService = siteStyleService;
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
            await _userService.GetByIdAsync(
                currentUserId.Value);

        if (currentUser == null)
        {
            return false;
        }

        return currentUser.Role ==
               Narrare.Domain.Enums.UserRole.Admin;
    }

    [HttpGet]
    public async Task<ActionResult<SiteStyle>> Get()
    {
        var style = await _siteStyleService.GetAsync();

        if (style == null)
        {
            return NotFound();
        }

        return Ok(style);
    }

    [HttpPost]
    public async Task<ActionResult<SiteStyle>> Create(
        [FromBody] SiteStyle siteStyle)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var existingStyle =
            await _siteStyleService.GetAsync();

        if (existingStyle != null)
        {
            return Conflict(
                "En stylingkonfiguration finns redan.");
        }

        var createdStyle =
            await _siteStyleService.CreateAsync(
                siteStyle);

        return Ok(createdStyle);
    }

    [HttpPut]
    public async Task<ActionResult<SiteStyle>> Update(
        [FromBody] SiteStyle siteStyle)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var updatedStyle =
            await _siteStyleService.UpdateAsync(
                siteStyle);

        if (updatedStyle == null)
        {
            return NotFound();
        }

        return Ok(updatedStyle);
    }
}