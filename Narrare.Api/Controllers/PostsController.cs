using Microsoft.AspNetCore.Mvc;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;

    public PostsController(
        IPostService postService,
        IUserService userService)
    {
        _postService = postService;
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

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] Post post)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        post.UserId = currentUserId.Value;

        var createdPost = await _postService.CreateAsync(post);

        var result = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Content = createdPost.Content,
            CreatedAt = createdPost.CreatedAt,
            UpdatedAt = createdPost.UpdatedAt,
            UserId = createdPost.UserId,
            CategoryId = createdPost.CategoryId
        };

        return Ok(result);
    }
    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetAll()
    {
        var posts = await _postService.GetAllAsync();

        var result = posts.Select(post => new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            UserId = post.UserId,
            CategoryId = post.CategoryId,
            IsDeleted = post.IsDeleted,
            DeletedByUserId = post.DeletedByUserId,
            DeletedAt = post.DeletedAt
        }).ToList();

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetById(int id)
    {
        var post = await _postService.GetByIdAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        var result = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt,
            UserId = post.UserId,
            CategoryId = post.CategoryId,
            IsDeleted = post.IsDeleted,
            DeletedByUserId = post.DeletedByUserId,
            DeletedAt = post.DeletedAt
        };

        return Ok(result);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<List<PostDto>>> GetByCategory(int categoryId)
    {
        var posts = await _postService.GetByCategoryAsync(categoryId);

        var result = posts.Select(post => new PostDto
{
    Id = post.Id,
    Title = post.Title,
    Content = post.Content,
    CreatedAt = post.CreatedAt,
    UpdatedAt = post.UpdatedAt,
    UserId = post.UserId,
    CategoryId = post.CategoryId,
    IsDeleted = post.IsDeleted,
    DeletedByUserId = post.DeletedByUserId,
    DeletedAt = post.DeletedAt
}).ToList();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> Update(
    int id,
    [FromBody] Post post)
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

        var existingPost = await _postService.GetByIdAsync(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        // Vanlig användare får bara ändra sina egna inlägg.
        // Admin får ändra alla inlägg.
        if (currentUser.Role != Narrare.Domain.Enums.UserRole.Admin &&
            existingPost.UserId != currentUser.Id)
        {
            return Forbid();
        }

        // Användaren får inte kunna byta ägare på inlägget.
        post.UserId = existingPost.UserId;
        post.Id = existingPost.Id;

        var updatedPost = await _postService.UpdateAsync(id, post);

        if (updatedPost == null)
        {
            return NotFound();
        }

        var result = new PostDto
        {
            Id = updatedPost.Id,
            Title = updatedPost.Title,
            Content = updatedPost.Content,
            CreatedAt = updatedPost.CreatedAt,
            UpdatedAt = updatedPost.UpdatedAt,
            UserId = updatedPost.UserId,
            CategoryId = updatedPost.CategoryId
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

        var post = await _postService.GetByIdAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        // Vanlig användare får bara ta bort sina egna inlägg.
        // Admin får ta bort alla inlägg.
        if (currentUser.Role != Narrare.Domain.Enums.UserRole.Admin &&
            post.UserId != currentUser.Id)
        {
            return Forbid();
        }

        post.IsDeleted = true;
        post.DeletedByUserId = currentUser.Id;
        post.DeletedAt = DateTime.UtcNow;

        await _postService.UpdateAsync(id, post);

        return NoContent();
    }
}



