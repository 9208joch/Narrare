using Narrare.Application.DTOs;
using Narrare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.Interfaces.Services;

public interface IContentReportService
{
    Task<bool> CreateAsync(
        CreateContentReportDto request,
        int reporterUserId);
    Task<List<ContentReportDto>> GetAllAsync();
    Task<bool> MarkAsReviewedAsync(int reportId);
}