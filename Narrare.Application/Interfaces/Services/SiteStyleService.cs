using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class SiteStyleService : ISiteStyleService
{
    private readonly ISiteStyleRepository _siteStyleRepository;

    public SiteStyleService(
        ISiteStyleRepository siteStyleRepository)
    {
        _siteStyleRepository = siteStyleRepository;
    }

    public async Task<SiteStyle?> GetAsync()
    {
        return await _siteStyleRepository.GetAsync();
    }

    public async Task<SiteStyle> CreateAsync(
        SiteStyle siteStyle)
    {
        return await _siteStyleRepository.CreateAsync(
            siteStyle);
    }

    public async Task<SiteStyle?> UpdateAsync(
        SiteStyle siteStyle)
    {
        return await _siteStyleRepository.UpdateAsync(
            siteStyle);
    }
}