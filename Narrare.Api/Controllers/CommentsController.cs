using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using System.Xml.Linq;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;

    public CommentsController(
        ICommentService commentService,
        IUserService userService)
    {
        _commentService = commentService;
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
    
    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetAll()
    {
        var comments = await _commentService.GetAllAsync();

        var result = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            UserId = comment.UserId,
            PostId = comment.PostId,
            IsDeleted = comment.IsDeleted,
            DeletedByUserId = comment.DeletedByUserId,
            DeletedAt = comment.DeletedAt
        }).ToList();

        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> Update(
     int id,
     [FromBody] Comment comment)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var currentUser = await _userService.GetByIdAsync(currentUserId.Value);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var existingComment = await _commentService.GetByIdAsync(id);

        if (existingComment == null)
        {
            return NotFound();
        }

        // Vanlig användare får bara ändra sina egna kommentarer.
        // Admin får ändra alla kommentarer.
        if (currentUser.Role != Narrare.Domain.Enums.UserRole.Admin &&
            existingComment.UserId != currentUser.Id)
        {
            return Forbid();
        }

        // Användaren får inte kunna byta ägare på kommentaren.
        comment.UserId = existingComment.UserId;
        comment.PostId = existingComment.PostId;
        comment.Id = existingComment.Id;

        var updatedComment = await _commentService.UpdateAsync(id, comment);

        if (updatedComment == null)
        {
            return NotFound();
        }
        
        var result = new CommentDto
        {
            Id = updatedComment.Id,
            Content = updatedComment.Content,
            CreatedAt = updatedComment.CreatedAt,
            UpdatedAt = updatedComment.UpdatedAt,
            UserId = updatedComment.UserId,
            PostId = updatedComment.PostId,
            IsDeleted = updatedComment.IsDeleted,
            DeletedByUserId = updatedComment.DeletedByUserId,
            DeletedAt = updatedComment.DeletedAt
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetById(int id)
    {
        var comment = await _commentService.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        var result = new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            UserId = comment.UserId,
            PostId = comment.PostId,
            IsDeleted = comment.IsDeleted,
            DeletedByUserId = comment.DeletedByUserId,
            DeletedAt = comment.DeletedAt
        };

        return Ok(result);
    }
    [HttpGet("post/{postId}")]
    public async Task<ActionResult<List<CommentDto>>> GetByPost(int postId)
    {
        var comments = await _commentService.GetByPostAsync(postId);
        
        var result = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            UserId = comment.UserId,
            PostId = comment.PostId,
            IsDeleted = comment.IsDeleted,
            DeletedByUserId = comment.DeletedByUserId,
            DeletedAt = comment.DeletedAt
        }).ToList();

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] Comment comment)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        // API:t bestämmer vem kommentaren tillhör.
        comment.UserId = currentUserId.Value;

        var createdComment = await _commentService.CreateAsync(comment);

        var result = new CommentDto
        {
            Id = createdComment.Id,
            Content = createdComment.Content,
            CreatedAt = createdComment.CreatedAt,
            UpdatedAt = createdComment.UpdatedAt,
            UserId = createdComment.UserId,
            PostId = createdComment.PostId
        };

        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var currentUser = await _userService.GetByIdAsync(currentUserId.Value);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var comment = await _commentService.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }
        
        // Vanlig användare får bara ta bort sina egna kommentarer.
        // Admin får ta bort alla kommentarer.
        if (currentUser.Role != Narrare.Domain.Enums.UserRole.Admin &&
            comment.UserId != currentUser.Id)
        {
            return Forbid();
        }

        // Soft delete
        comment.IsDeleted = true;
        comment.DeletedByUserId = currentUser.Id;
        comment.DeletedAt = DateTime.UtcNow;

        await _commentService.UpdateAsync(id, comment);

        return NoContent();
    }
}