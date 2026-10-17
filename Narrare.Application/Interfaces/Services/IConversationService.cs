using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface IConversationService
{
    Task<Conversation> CreateGroupAsync(
        int creatorId,
        string name,
        List<int> participantIds);

    Task<bool> RenameAsync(
        int conversationId,
        int userId,
        string name);

    Task AddParticipantsAsync(
        int conversationId,
        int requestingUserId,
        List<int> participantIds);

    Task<List<Conversation>> GetByUserIdAsync(int userId);
}
