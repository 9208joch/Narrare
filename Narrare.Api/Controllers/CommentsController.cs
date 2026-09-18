using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
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
            PostId = comment.PostId
        }).ToList();

        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<CommentDto>> Update(
    int id,
    [FromBody] Comment comment)
    {
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
            PostId = updatedComment.PostId
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
            PostId = comment.PostId
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
            PostId = comment.PostId
        }).ToList();

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] Comment comment)
    {
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
        var deleted = await _commentService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}