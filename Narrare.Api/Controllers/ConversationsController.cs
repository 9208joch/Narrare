
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using System.Linq;

namespace Narrare.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(
        IConversationService conversationService)
    {
        _conversationService = conversationService;
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
    
    [HttpPost("group")]
    public async Task<IActionResult> CreateGroup(
        [FromBody] CreateGroupConversationDto request)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var conversation = await _conversationService.CreateGroupAsync(
                currentUserId.Value,
                request.Name,
                request.ParticipantIds);

            return Ok(new ConversationDto
            {
                Id = conversation.Id,
                Name = conversation.Name,
                IsGroup = conversation.IsGroup,
                CreatedAt = conversation.CreatedAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{conversationId:int}/participants")]
    public async Task<IActionResult> AddParticipants(
    int conversationId,
    [FromBody] AddConversationParticipantsDto request)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        if (request.ParticipantIds == null ||
            request.ParticipantIds.Count == 0)
        {
            return BadRequest(new
            {
                message = "Välj minst en deltagare."
            });
        }

        try
        {
            await _conversationService.AddParticipantsAsync(
                conversationId,
                currentUserId.Value,
                request.ParticipantIds);

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{conversationId:int}/name")]
    public async Task<IActionResult> RenameConversation(
        int conversationId,
        [FromBody] RenameConversationDto request)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var success = await _conversationService.RenameAsync(
                conversationId,
                currentUserId.Value,
                request.Name);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet]
    public async Task<ActionResult<List<ConversationDto>>> GetMyConversations()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }
        

        var conversations = await _conversationService.GetByUserIdAsync(
            currentUserId.Value);

        var result = conversations
    .Select(conversation => new ConversationDto
    {
        Id = conversation.Id,
        Name = conversation.Name,
        IsGroup = conversation.IsGroup,
        CreatedAt = conversation.CreatedAt,
        MemberUsernames = conversation.Participants
            .Where(participant => participant.User != null)
            .Select(participant => participant.User!.Username)
            .OrderBy(username => username)
            .ToList()
    })
    .ToList();

        return Ok(result);
    }
    
}
