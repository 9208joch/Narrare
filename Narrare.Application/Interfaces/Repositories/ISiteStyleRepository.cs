using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface ISiteStyleRepository
{
    Task<SiteStyle?> GetAsync();

    Task<SiteStyle> CreateAsync(SiteStyle siteStyle);

    Task<SiteStyle?> UpdateAsync(SiteStyle siteStyle);
}