using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Repositories;

public interface IPrivateMessageRepository
{
    Task<PrivateMessage> CreateAsync(PrivateMessage message);
    Task<List<PrivateMessage>> GetInboxAsync(int userId);
    Task<List<PrivateMessage>> GetSentAsync(int userId);
    Task<PrivateMessage?> GetByIdAsync(int id);
    Task UpdateAsync(PrivateMessage message);
    Task<List<PrivateMessage>> GetConversationAsync(
    int userId,
    int otherUserId);
    Task<List<PrivateMessage>> GetByConversationIdAsync(
    int conversationId);

}
