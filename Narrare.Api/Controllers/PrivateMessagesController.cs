
using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrivateMessagesController : ControllerBase
{
    
    private readonly IPrivateMessageService _privateMessageService;
    private readonly IConversationRepository _conversationRepository;

    public PrivateMessagesController(
    IPrivateMessageService privateMessageService,
    IConversationRepository conversationRepository)
    {
        _privateMessageService = privateMessageService;
        _conversationRepository = conversationRepository;
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

    // Skicka ett privat meddelande.
    [HttpPost]
    public async Task<ActionResult<PrivateMessageDto>> Create(
        [FromBody] CreatePrivateMessageDto request)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var message = await _privateMessageService.CreateAsync(
                currentUserId.Value,
                request.ReceiverId,
                request.Content);

            return Ok(message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Hämta den inloggade användarens inkorg.
    [HttpGet("inbox")]
    public async Task<ActionResult<List<PrivateMessageDto>>> GetInbox()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var messages = await _privateMessageService.GetInboxAsync(
            currentUserId.Value);

        return Ok(messages);
    }

    // Hämta den inloggade användarens skickade meddelanden.
    [HttpGet("sent")]
    public async Task<ActionResult<List<PrivateMessageDto>>> GetSent()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var messages = await _privateMessageService.GetSentAsync(
            currentUserId.Value);

        return Ok(messages);
    }

    // Hämta alla meddelanden mellan den inloggade användaren
    // och en annan användare.
    [HttpGet("conversation/{otherUserId:int}")]
    public async Task<ActionResult<List<PrivateMessageDto>>> GetConversation(
        int otherUserId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        if (otherUserId <= 0 || otherUserId == currentUserId.Value)
        {
            return BadRequest(new
            {
                message = "Ange en giltig annan användare."
            });
        }

        var messages = await _privateMessageService.GetConversationAsync(
            currentUserId.Value,
            otherUserId);

        return Ok(messages);
    }


    // Markera ett mottaget meddelande som läst.
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var success = await _privateMessageService.MarkAsReadAsync(
            id,
            currentUserId.Value);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
    // Hämta meddelanden i en konversation.

    [HttpGet("by-conversation/{conversationId:int}")]
    public async Task<ActionResult<List<PrivateMessageDto>>> GetByConversationId(
        int conversationId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        if (conversationId <= 0)
        {
            return BadRequest(new
            {
                message = "Ange en giltig konversation."
            });
        }

        var isParticipant = await _conversationRepository.IsParticipantAsync(
            conversationId,
            currentUserId.Value);

        if (!isParticipant)
        {
            return NotFound();
        }

        var messages = await _privateMessageService.GetByConversationIdAsync(
            conversationId);

        return Ok(messages);
    }

    [HttpPost("group")]
    public async Task<ActionResult<PrivateMessageDto>> CreateGroupMessage(
        [FromBody] CreateGroupMessageDto request)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            var message = await _privateMessageService.CreateGroupMessageAsync(
                currentUserId.Value,
                request.ConversationId,
                request.Content);

            return Ok(message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("group-read-statuses")]
    public async Task<IActionResult> GetGroupReadStatuses()
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var statuses = await _privateMessageService
            .GetGroupReadStatusesAsync(currentUserId.Value);

        return Ok(statuses);
    }

    [HttpPut("by-conversation/{conversationId:int}/read")]
    public async Task<IActionResult> MarkGroupConversationAsRead(
        int conversationId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        try
        {
            await _privateMessageService.MarkGroupConversationAsReadAsync(
                conversationId,
                currentUserId.Value);

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return Forbid();
        }
    }


}
