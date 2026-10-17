using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Narrare.Application.DTOs;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class PrivateMessageService : IPrivateMessageService
{
    private readonly IPrivateMessageRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IConversationRepository _conversationRepository;
    private readonly IGroupMessageReadStatusRepository _groupMessageReadStatusRepository;
    
    public PrivateMessageService(
    IPrivateMessageRepository repository,
    IUserRepository userRepository,
    IConversationRepository conversationRepository,
    IGroupMessageReadStatusRepository groupMessageReadStatusRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
        _conversationRepository = conversationRepository;
        _groupMessageReadStatusRepository = groupMessageReadStatusRepository;
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
    public async Task<List<PrivateMessageDto>> GetByConversationIdAsync(
    int conversationId)
    {
        var messages = await _repository.GetByConversationIdAsync(
            conversationId);

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

    public async Task<PrivateMessageDto> CreateGroupMessageAsync(
        int senderId,
        int conversationId,
        string content)
    {
        var conversation = await _conversationRepository
    .GetByIdAsync(conversationId);

        if (conversation == null || !conversation.IsGroup)
        {
            throw new InvalidOperationException(
                "Gruppkonversationen kunde inte hittas.");
        }

        var isParticipant = await _conversationRepository
            .IsParticipantAsync(conversationId, senderId);

        if (!isParticipant)
        {
            throw new InvalidOperationException(
                "Du är inte deltagare i den här gruppen.");
        }
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new InvalidOperationException(
                "Meddelandet får inte vara tomt.");
        }

        if (content.Length > 5000)
        {
            throw new InvalidOperationException(
                "Meddelandet får vara högst 5000 tecken.");
        }

        var sender = await _userRepository.GetByIdAsync(senderId);

        if (sender == null)
        {
            throw new InvalidOperationException(
                "Avsändaren kunde inte hittas.");
        }

        var message = new PrivateMessage
        {
            SenderId = senderId,
            ReceiverId = senderId,
            ConversationId = conversationId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow,
            IsRead = false
        };


        var createdMessage = await _repository.CreateAsync(message);

        // Hämta gruppens deltagare.
        var participants = conversation.Participants;

        // Skapa en lässtatus för varje deltagare utom avsändaren.
        var readStatuses = participants
            .Where(participant => participant.UserId != senderId)
            .Select(participant => new GroupMessageReadStatus
            {
                MessageId = createdMessage.Id,
                UserId = participant.UserId,
                IsRead = false
            })
            .ToList();

        // Spara deltagarnas olästa statusar.
        await _groupMessageReadStatusRepository.CreateRangeAsync(
            readStatuses);

        return MapToDto(
            createdMessage,
            sender.Username,
            sender.Username);

    }

    public async Task<List<GroupMessageReadStatus>>
        GetGroupReadStatusesAsync(int userId)
    {
        return await _groupMessageReadStatusRepository
            .GetByUserIdAsync(userId);
    }

    public async Task MarkGroupConversationAsReadAsync(
        int conversationId,
        int userId)
    {
        var isParticipant = await _conversationRepository
            .IsParticipantAsync(conversationId, userId);

        if (!isParticipant)
        {
            throw new InvalidOperationException(
                "Du är inte deltagare i den här gruppen.");
        }

        await _groupMessageReadStatusRepository
            .MarkConversationAsReadAsync(conversationId, userId);
    }


}
