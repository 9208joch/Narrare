using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PageContentController : ControllerBase
{
    private readonly IPageContentService _pageContentService;
    private readonly IUserService _userService;

    public PageContentController(
        IPageContentService pageContentService,
        IUserService userService)
    {
        _pageContentService = pageContentService;
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

        return currentUser.Role ==
               Narrare.Domain.Enums.UserRole.Admin;
    }


    // ========================================
    // ALLA FÅR LÄSA
    // ========================================

    [HttpGet("page/{pageId:int}")]
    public async Task<ActionResult<List<PageContent>>> GetByPageId(
        int pageId)
    {
        var content = await _pageContentService
            .GetByPageIdAsync(pageId);

        return Ok(content);
    }


    [HttpGet("{id:int}")]
    public async Task<ActionResult<PageContent>> GetById(int id)
    {
        var content = await _pageContentService
            .GetByIdAsync(id);

        if (content == null)
        {
            return NotFound();
        }

        return Ok(content);
    }


    // ========================================
    // ENDAST ADMIN FÅR SKAPA
    // ========================================

    [HttpPost]
    public async Task<ActionResult<PageContent>> Create(
        [FromBody] PageContent content)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var createdContent =
            await _pageContentService.CreateAsync(content);

        return Ok(createdContent);
    }


    // ========================================
    // ENDAST ADMIN FÅR ÄNDRA
    // ========================================

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PageContent>> Update(
        int id,
        [FromBody] PageContent content)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var existingContent =
            await _pageContentService.GetByIdAsync(id);

        if (existingContent == null)
        {
            return NotFound();
        }

        content.Id = id;

        var updatedContent =
            await _pageContentService.UpdateAsync(content);

        if (updatedContent == null)
        {
            return NotFound();
        }

        return Ok(updatedContent);
    }


    // ========================================
    // ENDAST ADMIN FÅR RADERA
    // ========================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await IsAdmin())
        {
            return Forbid();
        }

        var deleted =
            await _pageContentService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}