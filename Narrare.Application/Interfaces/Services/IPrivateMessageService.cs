using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.DTOs;

namespace Narrare.Application.Interfaces.Services;

public interface IPrivateMessageService
{
    Task<PrivateMessageDto> CreateAsync(
        int senderId,
        int receiverId,
        string content);

    Task<List<PrivateMessageDto>> GetInboxAsync(int userId);

    Task<List<PrivateMessageDto>> GetSentAsync(int userId);

    Task<bool> MarkAsReadAsync(int messageId, int userId);
    Task<List<PrivateMessageDto>> GetConversationAsync(
    int userId,
    int otherUserId);
}
