using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class SiteStyleRepository : ISiteStyleRepository
{
    private readonly NarrareDbContext _context;

    public SiteStyleRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<SiteStyle?> GetAsync()
    {
        return await _context.SiteStyles
            .FirstOrDefaultAsync();
    }

    public async Task<SiteStyle> CreateAsync(SiteStyle siteStyle)
    {
        _context.SiteStyles.Add(siteStyle);

        await _context.SaveChangesAsync();

        return siteStyle;
    }

    public async Task<SiteStyle?> UpdateAsync(SiteStyle siteStyle)
    {
        var existingStyle = await _context.SiteStyles
            .FirstOrDefaultAsync();

        if (existingStyle == null)
        {
            return null;
        }

        existingStyle.HeadingFontSize =
            siteStyle.HeadingFontSize;

        existingStyle.HeadingBold =
            siteStyle.HeadingBold;

        existingStyle.HeadingColor =
            siteStyle.HeadingColor;

        existingStyle.HeadingMarginBottom =
            siteStyle.HeadingMarginBottom;

        existingStyle.TextFontSize =
            siteStyle.TextFontSize;

        existingStyle.TextColor =
            siteStyle.TextColor;

        existingStyle.TextLineHeight =
            siteStyle.TextLineHeight;

        existingStyle.TextMarginBottom =
            siteStyle.TextMarginBottom;

        existingStyle.ImageMaxWidth =
            siteStyle.ImageMaxWidth;

        existingStyle.ImageBorderRadius =
            siteStyle.ImageBorderRadius;

        existingStyle.LinkColor =
            siteStyle.LinkColor;

        existingStyle.LinkBold =
            siteStyle.LinkBold;

        existingStyle.LinkUnderline =
            siteStyle.LinkUnderline;

        await _context.SaveChangesAsync();

        return existingStyle;
    }
}