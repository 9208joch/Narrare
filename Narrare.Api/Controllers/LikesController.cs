using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly ILikeService _likeService;
    private readonly IUserService _userService;

    public LikesController(
        ILikeService likeService,
        IUserService userService)
    {
        _likeService = likeService;
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

    
    [HttpGet("post/{postId}")]
    public async Task<IActionResult> GetByPost(int postId)
    {
        var likes = await _likeService.GetByPostAsync(postId);

        var currentUserId = GetCurrentUserId();
        var isLiked = false;

        if (currentUserId != null)
        {
            var currentUser =
                await _userService.GetByIdAsync(currentUserId.Value);

            if (currentUser != null)
            {
                isLiked = likes.Any(like =>
                    like.UserId == currentUser.Id);
            }
        }

        return Ok(new
        {
            Count = likes.Count,
            IsLiked = isLiked
        });
    }


    [HttpGet("comment/{commentId}")]
    public async Task<IActionResult> GetByComment(int commentId)
    {
        var likes = await _likeService.GetByCommentAsync(commentId);

        var currentUserId = GetCurrentUserId();
        var isLiked = false;

        if (currentUserId != null)
        {
            var currentUser =
                await _userService.GetByIdAsync(currentUserId.Value);

            if (currentUser != null)
            {
                isLiked = likes.Any(like =>
                    like.UserId == currentUser.Id);
            }
        }

        return Ok(new
        {
            Count = likes.Count,
            IsLiked = isLiked
        });
    }

    [HttpPost("post/{postId}/toggle")]
    public async Task<IActionResult> TogglePostLike(int postId)
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

        var isLiked = await _likeService.TogglePostLikeAsync(
            currentUser.Id,
            postId);

        var likes = await _likeService.GetByPostAsync(postId);

        return Ok(new
        {
            IsLiked = isLiked,
            Count = likes.Count
        });
    }

    [HttpPost("comment/{commentId}/toggle")]
    public async Task<IActionResult> ToggleCommentLike(int commentId)
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

        var isLiked = await _likeService.ToggleCommentLikeAsync(
            currentUser.Id,
            commentId);

        var likes = await _likeService.GetByCommentAsync(commentId);

        return Ok(new
        {
            IsLiked = isLiked,
            Count = likes.Count
        });
    }




}
