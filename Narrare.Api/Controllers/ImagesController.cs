
using Microsoft.AspNetCore.Mvc;
using Narrare.Application.Interfaces.Services;
using Narrare.Application.Services;
using System.Linq;

namespace Narrare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly IUserService _userService;
    private readonly IImageStorageService _imageStorageService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;

    public ImagesController(
    IImageService imageService,
    IUserService userService,
    IImageStorageService imageStorageService,
    IPostService postService,
    ICommentService commentService)
    {
        _imageService = imageService;
        _userService = userService;
        _imageStorageService = imageStorageService;
        _postService = postService;
        _commentService = commentService;
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
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(21 * 1024 * 1024)]
    public async Task<IActionResult> Upload(IFormFile? file)
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

        if (file == null || file.Length == 0)
        {
            return BadRequest("Ingen bild har valts.");
        }

        const long maxFileSize = 20 * 1024 * 1024;

        if (file.Length > maxFileSize)
        {
            return BadRequest("Bilden får inte vara större än 20 MB.");
        }

        await using var stream = file.OpenReadStream();

        try
        {
            var uploadedImage = await _imageService.UploadAsync(
                stream,
                file.ContentType,
                file.Length,
                currentUser.Id);

            return Ok(new
            {
                uploadedImage.Id,
                uploadedImage.FileName,
                uploadedImage.ContentType,
                uploadedImage.FileSize,
                uploadedImage.UploadedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("gallery")]
    public async Task<IActionResult> GetGalleryImages()
    {
        var images = await _imageService.GetGalleryImagesAsync();

        var result = images.Select(usage => new
        {
            usage.Id,
            ImageId = usage.UploadedImageId,
            usage.UploadedImage!.FileName,
            usage.UploadedImage.ContentType,
            usage.SortOrder
        });

        return Ok(result);
    }

    [HttpPost("gallery/{imageId:int}")]
    public async Task<IActionResult> AddToGallery(int imageId)
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

        // Tillfällig kontroll: endast användare med Role = 1 får lägga till bilder.
        if ((int)currentUser.Role != 1)
        {
            return Forbid();
        }

        try
        {
            await _imageService.AddToGalleryAsync(imageId);
            return Ok(new { Message = "Bilden har lagts till i galleriet." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpDelete("gallery/{imageId:int}")]
    public async Task<IActionResult> RemoveFromGallery(int imageId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var currentUser = await _userService.GetByIdAsync(
            currentUserId.Value);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        // Tillfällig kontroll: endast Role = 1 får ändra galleriet.
        if ((int)currentUser.Role != 1)
        {
            return Forbid();
        }

        await _imageService.RemoveFromGalleryAsync(imageId);

        return Ok(new
        {
            Message = "Bilden har tagits bort från galleriet."
        });
    }
    [HttpPost("posts/{postId:int}/images/{imageId:int}")]
    public async Task<IActionResult> AddImageToPost(
        int postId,
        int imageId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var currentUser = await _userService.GetByIdAsync(
            currentUserId.Value);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var post = await _postService.GetByIdAsync(postId);

        if (post == null)
        {
            return NotFound("Inlägget kunde inte hittas.");
        }

        if (post.UserId != currentUser.Id)
        {
            return Forbid();
        }

        try
        {
            await _imageService.AddToPostAsync(imageId, postId);

            return Ok(new
            {
                Message = "Bilden har kopplats till inlägget."
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("posts/{postId:int}/images")]
    public async Task<IActionResult> GetPostImages(int postId)
    {
        // Kontrollera att inlägget finns.
        var post = await _postService.GetByIdAsync(postId);

        if (post == null)
        {
            return NotFound("Inlägget kunde inte hittas.");
        }

        // Hämta bilderna som är kopplade till inlägget.
        var images = await _imageService.GetByPostAsync(postId);

        // Skapa ett svar med information om bilderna.
        var result = images.Select(usage => new
        {
            usage.Id,
            ImageId = usage.UploadedImageId,
            usage.UploadedImage!.FileName,
            usage.UploadedImage.ContentType,
            usage.UploadedImage.FileSize,
            usage.SortOrder
        });
        
        return Ok(result);
    }
    [HttpPost("comments/{commentId:int}/images/{imageId:int}")]
    public async Task<IActionResult> AddImageToComment(
        int commentId,
        int imageId)
    {
        var currentUserId = GetCurrentUserId();

        if (currentUserId == null)
        {
            return Unauthorized();
        }

        var currentUser = await _userService.GetByIdAsync(
            currentUserId.Value);

        if (currentUser == null)
        {
            return Unauthorized();
        }

        var comment = await _commentService.GetByIdAsync(commentId);

        if (comment == null)
        {
            return NotFound("Kommentaren hittades inte.");
        }

        var post = await _postService.GetByIdAsync(comment.PostId);

        if (post == null)
        {
            return NotFound("Inlägget hittades inte.");
        }

        if (post.UserId != currentUser.Id)
        {
            return Forbid();
        }

        try
        {
            await _imageService.AddToCommentAsync(imageId, commentId);

            return Ok(new
            {
                Message = "Bilden har kopplats till kommentaren."
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpGet("comments/{commentId:int}/images")]
    public async Task<IActionResult> GetCommentImages(int commentId)
    {
        // Kontrollera att kommentaren finns.
        var comment = await _commentService.GetByIdAsync(commentId);

        if (comment == null)
        {
            return NotFound("Kommentaren hittades inte.");
        }

        // Hämta bilderna som är kopplade till kommentaren.
        var images = await _imageService.GetByCommentAsync(commentId);

        // Skapa svaret med information om bilderna.
        var result = images.Select(usage => new
        {
            usage.Id,
            ImageId = usage.UploadedImageId,
            usage.UploadedImage!.FileName,
            usage.UploadedImage.ContentType,
            usage.UploadedImage.FileSize,
            usage.SortOrder
        });

        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await _imageService.GetByIdAsync(id);

        if (image == null)
        {
            return NotFound("Bilden kunde inte hittas.");
        }

        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "UploadedImages",
            image.FileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Bildfilen kunde inte hittas.");
        }

        return PhysicalFile(
            filePath,
            "image/jpeg");
    }


}
