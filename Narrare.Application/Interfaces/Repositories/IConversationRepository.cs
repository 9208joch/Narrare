using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IConversationRepository
{
    Task<Conversation?> GetByIdAsync(int conversationId);

    Task<bool> IsParticipantAsync(
        int conversationId,
        int userId);
    Task<Conversation> CreateAsync(Conversation conversation);

    Task UpdateAsync(Conversation conversation);
    Task AddParticipantsAsync(
    List<ConversationParticipant> participants);
    Task<List<Conversation>> GetByUserIdAsync(int userId);
}