using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly IPageService _pageService;

    public PagesController(IPageService pageService)
    {
        _pageService = pageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Page>>> GetAll()
    {
        var pages = await _pageService.GetAllAsync();

        return Ok(pages);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Page>> GetById(int id)
    {
        var page = await _pageService.GetByIdAsync(id);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<Page>> GetBySlug(string slug)
    {
        var page = await _pageService.GetBySlugAsync(slug);

        if (page == null)
        {
            return NotFound();
        }

        return Ok(page);
    }

    [HttpPost]
    public async Task<ActionResult<Page>> Create([FromBody] Page page)
    {
        var createdPage = await _pageService.CreateAsync(page);

        return Ok(createdPage);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Page>> Update(
        int id,
        [FromBody] Page page)
    {
        var existingPage = await _pageService.GetByIdAsync(id);

        if (existingPage == null)
        {
            return NotFound();
        }

        page.Id = id;

        var updatedPage = await _pageService.UpdateAsync(page);

        if (updatedPage == null)
        {
            return NotFound();
        }

        return Ok(updatedPage);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pageService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}