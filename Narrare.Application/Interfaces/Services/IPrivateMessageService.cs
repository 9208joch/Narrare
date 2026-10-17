using Narrare.Application.DTOs;
using Narrare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
    Task<List<PrivateMessageDto>> GetByConversationIdAsync(
    int conversationId);
    Task<PrivateMessageDto> CreateGroupMessageAsync(
    int senderId,
    int conversationId,
    string content);
    Task<List<GroupMessageReadStatus>> GetGroupReadStatusesAsync(
    int userId);

    Task MarkGroupConversationAsReadAsync(
        int conversationId,
        int userId);
}
