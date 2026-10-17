using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class GroupMessageReadStatusRepository
    : IGroupMessageReadStatusRepository
{
    
    private readonly NarrareDbContext _context;

    public GroupMessageReadStatusRepository(
        NarrareDbContext context)
    {
        _context = context;
    }

    public async Task CreateRangeAsync(
        List<GroupMessageReadStatus> statuses)
    {
        if (statuses.Count == 0)
        {
            return;
        }

        _context.GroupMessageReadStatuses.AddRange(statuses);
        await _context.SaveChangesAsync();
    }

    public async Task<List<GroupMessageReadStatus>> GetByUserIdAsync(
    int userId)
    {
        return await _context.GroupMessageReadStatuses
            .Include(status => status.Message)
            .Where(status => status.UserId == userId)
            .ToListAsync();
    }

    public async Task MarkConversationAsReadAsync(
        int conversationId,
        int userId)
    {
        var statuses = await _context.GroupMessageReadStatuses
            .Where(status =>
                status.UserId == userId &&
                status.Message != null &&
                status.Message.ConversationId == conversationId &&
                !status.IsRead)
            .ToListAsync();

        foreach (var status in statuses)
        {
            status.IsRead = true;
        }

        if (statuses.Count > 0)
        {
            await _context.SaveChangesAsync();
        }
    }
}
