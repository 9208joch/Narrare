using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Domain.Entities;
using Narrare.Infrastructure.Data;

namespace Narrare.Infrastructure.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly NarrareDbContext _context;

    public ConversationRepository(NarrareDbContext context)
    {
        _context = context;
    }


    public async Task<Conversation?> GetByIdAsync(int conversationId)
    {
        return await _context.Conversations
            .Include(conversation => conversation.Participants)
            .FirstOrDefaultAsync(
                conversation => conversation.Id == conversationId);
    }


    public async Task<bool> IsParticipantAsync(
        int conversationId,
        int userId)
    {
        return await _context.ConversationParticipants
            .AnyAsync(participant =>
                participant.ConversationId == conversationId &&
                participant.UserId == userId);
    }
    public async Task<Conversation> CreateAsync(Conversation conversation)
    {
        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return conversation;
    }

    public async Task UpdateAsync(Conversation conversation)
    {
        _context.Conversations.Update(conversation);
        await _context.SaveChangesAsync();
    }
    public async Task AddParticipantsAsync(
    List<ConversationParticipant> participants)
    {
        _context.ConversationParticipants.AddRange(participants);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Conversation>> GetByUserIdAsync(int userId)
    {
        return await _context.Conversations
            .Where(conversation =>
                conversation.Participants.Any(
                    participant => participant.UserId == userId))
            .Include(conversation => conversation.Participants)
                .ThenInclude(participant => participant.User)
            .OrderByDescending(conversation => conversation.CreatedAt)
            .ToListAsync();
    }

}
