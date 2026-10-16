using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class PrivateMessageService : IPrivateMessageService
{
    private readonly IPrivateMessageRepository _repository;
    private readonly IUserRepository _userRepository;
    
    public PrivateMessageService(
        IPrivateMessageRepository repository,
        IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<PrivateMessageDto> CreateAsync(
        int senderId,
        int receiverId,
        string content)
    {
        var sender = await _userRepository.GetByIdAsync(senderId);
        var receiver = await _userRepository.GetByIdAsync(receiverId);

        if (sender == null || receiver == null)
        {
            throw new InvalidOperationException(
                "Avsändaren eller mottagaren kunde inte hittas.");
        }

        if (senderId == receiverId)
        {
            throw new InvalidOperationException(
                "Du kan inte skicka ett privat meddelande till dig själv.");
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(
                "Meddelandet får inte vara tomt.");
        }

        var message = new PrivateMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow,
            IsRead = false
        };

        var createdMessage = await _repository.CreateAsync(message);

        return MapToDto(createdMessage, sender.Username, receiver.Username);
    }

    public async Task<List<PrivateMessageDto>> GetConversationAsync(
        int userId,
        int otherUserId)
    {
        var messages = await _repository.GetConversationAsync(
            userId,
            otherUserId);

        var result = new List<PrivateMessageDto>();

        foreach (var message in messages)
        {
            var sender = await _userRepository.GetByIdAsync(message.SenderId);
            var receiver = await _userRepository.GetByIdAsync(message.ReceiverId);

            if (sender != null && receiver != null)
            {
                result.Add(MapToDto(
                    message,
                    sender.Username,
                    receiver.Username));
            }
        }

        return result;
    }


    public async Task<List<PrivateMessageDto>> GetInboxAsync(int userId)
    {
        var messages = await _repository.GetInboxAsync(userId);
        var result = new List<PrivateMessageDto>();

        foreach (var message in messages)
        {
            var sender = await _userRepository.GetByIdAsync(message.SenderId);
            var receiver = await _userRepository.GetByIdAsync(message.ReceiverId);

            if (sender != null && receiver != null)
            {
                result.Add(MapToDto(message, sender.Username, receiver.Username));
            }
        }

        return result;
    }

    public async Task<List<PrivateMessageDto>> GetSentAsync(int userId)
    {
        var messages = await _repository.GetSentAsync(userId);
        var result = new List<PrivateMessageDto>();

        foreach (var message in messages)
        {
            var sender = await _userRepository.GetByIdAsync(message.SenderId);
            var receiver = await _userRepository.GetByIdAsync(message.ReceiverId);

            if (sender != null && receiver != null)
            {
                result.Add(MapToDto(message, sender.Username, receiver.Username));
            }
        }

        return result;
    }


    public async Task<bool> MarkAsReadAsync(int messageId, int userId)
    {
        var message = await _repository.GetByIdAsync(messageId);

        if (message == null || message.ReceiverId != userId)
        {
            return false;
        }

        if (!message.IsRead)
        {
            message.IsRead = true;
            await _repository.UpdateAsync(message);
        }

        return true;
    }


    private static PrivateMessageDto MapToDto(
        PrivateMessage message,
        string senderUsername,
        string receiverUsername)
    {
        return new PrivateMessageDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            SenderUsername = senderUsername,
            ReceiverId = message.ReceiverId,
            ReceiverUsername = receiverUsername,
            Content = message.Content,
            SentAt = message.SentAt,
            IsRead = message.IsRead
        };
    }
}
