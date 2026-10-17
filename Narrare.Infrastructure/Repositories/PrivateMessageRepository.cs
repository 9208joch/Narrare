using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class PrivateMessageRepository : IPrivateMessageRepository
{
    private readonly NarrareDbContext _context;

    public PrivateMessageRepository(NarrareDbContext context)
    {
        _context = context;
    }

    public async Task<PrivateMessage> CreateAsync(PrivateMessage message)
    {
        _context.PrivateMessages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    public async Task<List<PrivateMessage>> GetInboxAsync(int userId)
    {
        return await _context.PrivateMessages
            .Where(message => message.ReceiverId == userId)
            .OrderByDescending(message => message.SentAt)
            .ToListAsync();
    }

    public async Task<List<PrivateMessage>> GetSentAsync(int userId)
    {
        return await _context.PrivateMessages
            .Where(message => message.SenderId == userId)
            .OrderByDescending(message => message.SentAt)
            .ToListAsync();
    }

    public async Task<PrivateMessage?> GetByIdAsync(int id)
    {
        return await _context.PrivateMessages
            .FirstOrDefaultAsync(message => message.Id == id);
    }
    public async Task UpdateAsync(PrivateMessage message)
    {
        _context.PrivateMessages.Update(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PrivateMessage>> GetConversationAsync(
        int userId,
        int otherUserId)
    {
        return await _context.PrivateMessages
            .Where(message =>
                (message.SenderId == userId &&
                 message.ReceiverId == otherUserId)
                ||
                (message.SenderId == otherUserId &&
                 message.ReceiverId == userId))
            .OrderBy(message => message.SentAt)
            .ToListAsync();
    }
    public async Task<List<PrivateMessage>> GetByConversationIdAsync(
    int conversationId)
    {
        return await _context.PrivateMessages
            .Where(message => message.ConversationId == conversationId)
            .OrderBy(message => message.SentAt)
            .ToListAsync();
    }

}
