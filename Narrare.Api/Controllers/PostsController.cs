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

    public PostsController(IPostService postService)
    {
        _postService = postService;
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
            CategoryId = post.CategoryId
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
            CategoryId = post.CategoryId
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
            CategoryId = post.CategoryId
        }).ToList();

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] Post post)
    {
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
    [HttpPut("{id}")]
    public async Task<ActionResult<PostDto>> Update(
    int id,
    [FromBody] Post post)
    {
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
            UserId = updatedPost.UserId,
            CategoryId = updatedPost.CategoryId
        };

        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _postService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}