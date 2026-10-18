using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces;

public interface IContentReportRepository
{
    Task<ContentReport> CreateAsync(ContentReport report);

    // Hämtar alla anmälningar för administratörernas granskningslista.
    Task<List<ContentReport>> GetAllAsync();
    Task<bool> MarkAsReviewedAsync(int reportId);
}
