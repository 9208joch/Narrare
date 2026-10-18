
using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Enums;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContentReportsController : ControllerBase
{
    private readonly IContentReportService _contentReportService;
    private readonly IUserService _userService;
    
    public ContentReportsController(
    IContentReportService contentReportService,
    IUserService userService)
    {
        _contentReportService = contentReportService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContentReportDto request)
    {
        // Hämta användar-ID från den befintliga headern.
        var userIdHeader = Request.Headers["X-User-Id"].FirstOrDefault();

        if (!int.TryParse(userIdHeader, out int userId))
        {
            return Unauthorized();
        }

        bool success = await _contentReportService.CreateAsync(
            request,
            userId);

        if (!success)
        {
            return BadRequest(
                "Anmälan är ogiltig. Kontrollera innehållet och motiveringen.");
        }

        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Hämta användarens ID från request-headern.
        var userIdHeader = Request.Headers["X-User-Id"].FirstOrDefault();

        if (!int.TryParse(userIdHeader, out int userId))
            return Unauthorized();

        // Hämta den verkliga användaren från databasen.
        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        // Endast administratörer får se anmälningarna.
        if (user.Role != UserRole.Admin)
            return Forbid();

        // Hämta alla anmälningar.
        var reports = await _contentReportService.GetAllAsync();

        return Ok(reports);
    }

    [HttpPut("{reportId:int}/review")]
    public async Task<IActionResult> MarkAsReviewed(int reportId)
    {
        var userIdHeader = Request.Headers["X-User-Id"].FirstOrDefault();

        if (!int.TryParse(userIdHeader, out int userId))
            return Unauthorized();

        var user = await _userService.GetByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        if (user.Role != UserRole.Admin)
            return Forbid();

        bool success =
            await _contentReportService.MarkAsReviewedAsync(reportId);

        if (!success)
            return NotFound("Anmälningen hittades inte.");

        return NoContent();
    }



}
