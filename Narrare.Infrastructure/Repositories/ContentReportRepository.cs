using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Infrastructure.Repositories;

public class ContentReportRepository : IContentReportRepository
{
    private readonly NarrareDbContext _context;

    public ContentReportRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<ContentReport> CreateAsync(ContentReport report)
    {
        _context.ContentReports.Add(report);

        await _context.SaveChangesAsync();

        return report;
    }

    public async Task<bool> MarkAsReviewedAsync(int reportId)
    {
        var report = await _context.ContentReports
            .FirstOrDefaultAsync(report => report.Id == reportId);

        if (report == null)
        {
            return false;
        }

        report.IsReviewed = true;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ContentReport>> GetAllAsync()
    {
        return await _context.ContentReports
            .Include(report => report.ReporterUser)
            .Include(report => report.Post)
            .Include(report => report.Comment)
            .OrderByDescending(report => report.CreatedAt)
            .ToListAsync();
    }

}