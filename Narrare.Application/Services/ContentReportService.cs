
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using System.ComponentModel.Design;
using System.Linq;

namespace Narrare.Application.Services;

public class ContentReportService : IContentReportService
{
    private readonly IContentReportRepository _repository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public ContentReportService(
        IContentReportRepository repository,
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        _repository = repository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }
    
    public async Task<bool> CreateAsync(
        CreateContentReportDto request,
        int reporterUserId)
    {
        // Kontrollera att anmälan gäller exakt ett innehåll.
        bool reportsPost = request.PostId.HasValue;
        bool reportsComment = request.CommentId.HasValue;

        if (reportsPost == reportsComment)
        {
            return false;
        }

        // Kontrollera att det anmälda inlägget finns.
        if (request.PostId.HasValue)
        {
            var post = await _postRepository.GetByIdAsync(
                request.PostId.Value);

            if (post == null)
            {
                return false;
            }
        }

        // Kontrollera att den anmälda kommentaren finns.
        if (request.CommentId.HasValue)
        {
            var comment = await _commentRepository.GetByIdAsync(
                request.CommentId.Value);

            if (comment == null)
            {
                return false;
            }
        }

        // Kontrollera att motiveringen inte är tom.
        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return false;
        }

        // Ta bort onödiga blanksteg och kontrollera längden.
        string reason = request.Reason.Trim();

        if (reason.Length > 1000)
        {
            return false;
        }

        // Skapa anmälan.
        var report = new ContentReport
        {
            ReporterUserId = reporterUserId,
            PostId = request.PostId,
            CommentId = request.CommentId,
            Reason = reason,
            CreatedAt = DateTime.UtcNow,
            IsReviewed = false
        };

        // Spara anmälan i databasen.
        await _repository.CreateAsync(report);

        return true;
    }


    public async Task<List<ContentReportDto>> GetAllAsync()
    {
        var reports = await _repository.GetAllAsync();
        
        return reports.Select(report => new ContentReportDto
        {
            Id = report.Id,
            ReporterUserId = report.ReporterUserId,
            ReporterUsername = report.ReporterUser?.Username,
            PostId = report.PostId,
            PostTitle = report.Post?.Title,
            PostIsDeleted = report.Post?.IsDeleted ?? false,
            CommentId = report.CommentId,
            CommentContent = report.Comment?.Content,
            CommentIsDeleted = report.Comment?.IsDeleted ?? false,
            Reason = report.Reason,
            CreatedAt = report.CreatedAt,
            IsReviewed = report.IsReviewed
        }).ToList();
    }
    
    public async Task<bool> MarkAsReviewedAsync(int reportId)
    {
        return await _repository.MarkAsReviewedAsync(reportId);
    }



}
