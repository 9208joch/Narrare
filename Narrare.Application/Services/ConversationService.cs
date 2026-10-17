using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;

namespace Narrare.Application.Services;

public class ConversationService : IConversationService
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IUserRepository _userRepository;

    public ConversationService(
        IConversationRepository conversationRepository,
        IUserRepository userRepository)
    {
        _conversationRepository = conversationRepository;
        _userRepository = userRepository;
    }


    public async Task<Conversation> CreateGroupAsync(
        int creatorId,
        string name,
        List<int> participantIds)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException(
                "Gruppen måste ha ett namn.");
        }

        name = name.Trim();

        if (name.Length > 100)
        {
            throw new InvalidOperationException(
                "Gruppnamnet får vara högst 100 tecken.");
        }

        var creator = await _userRepository.GetByIdAsync(creatorId);

        if (creator == null)
        {
            throw new InvalidOperationException(
                "Den som skapar gruppen kunde inte hittas.");
        }

        var allParticipantIds = participantIds
            .Append(creatorId)
            .Distinct()
            .ToList();

        foreach (var participantId in allParticipantIds)
        {
            var user = await _userRepository.GetByIdAsync(participantId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    $"Användaren med ID {participantId} kunde inte hittas.");
            }
        }

        if (allParticipantIds.Count < 2)
        {
            throw new InvalidOperationException(
                "En grupp måste ha minst två deltagare.");
        }

        var conversation = new Conversation
        {
            Name = name,
            IsGroup = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var createdConversation =
            await _conversationRepository.CreateAsync(conversation);

        var participants = allParticipantIds
            .Select(userId => new ConversationParticipant
            {
                ConversationId = createdConversation.Id,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            })
            .ToList();

        await _conversationRepository.AddParticipantsAsync(participants);

        return createdConversation;
    }




    public async Task<bool> RenameAsync(
        int conversationId,
        int userId,
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException(
                "Gruppnamnet får inte vara tomt.");
        }

        name = name.Trim();

        if (name.Length > 100)
        {
            throw new InvalidOperationException(
                "Gruppnamnet får vara högst 100 tecken.");
        }

        var conversation = await _conversationRepository
            .GetByIdAsync(conversationId);

        if (conversation == null || !conversation.IsGroup)
        {
            return false;
        }

        var isParticipant = await _conversationRepository
            .IsParticipantAsync(conversationId, userId);

        if (!isParticipant)
        {
            return false;
        }

        conversation.Name = name;

        await _conversationRepository.UpdateAsync(conversation);

        return true;
    }

    public async Task AddParticipantsAsync(
        int conversationId,
        int requestingUserId,
        List<int> participantIds)
    {
        var conversation = await _conversationRepository
            .GetByIdAsync(conversationId);

        if (conversation == null || !conversation.IsGroup)
        {
            throw new InvalidOperationException(
                "Det går bara att bjuda in deltagare till en gruppchatt.");
        }

        var isParticipant = await _conversationRepository
            .IsParticipantAsync(conversationId, requestingUserId);

        if (!isParticipant)
        {
            throw new InvalidOperationException(
                "Du måste vara medlem i gruppchatten för att bjuda in deltagare.");
        }

        if (participantIds == null || participantIds.Count == 0)
        {
            throw new InvalidOperationException(
                "Välj minst en deltagare att bjuda in.");
        }

        // Ta bort dubbletter och användare som redan är medlemmar.
        var existingParticipants = await _conversationRepository
            .GetByUserIdAsync(requestingUserId);

        var existingConversation = existingParticipants
            .FirstOrDefault(c => c.Id == conversationId);

        var existingParticipantIds = existingConversation?.Participants
            .Select(p => p.UserId)
            .ToHashSet() ?? new HashSet<int>();

        var newParticipantIds = participantIds
            .Distinct()
            .Where(id => !existingParticipantIds.Contains(id))
            .ToList();

        foreach (var participantId in newParticipantIds)
        {
            var user = await _userRepository.GetByIdAsync(participantId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    $"Användaren med ID {participantId} kunde inte hittas.");
            }
        }

        if (newParticipantIds.Count == 0)
        {
            throw new InvalidOperationException(
                "Alla valda användare är redan med i gruppchatten.");
        }
        
        var participants = newParticipantIds
            .Select(userId => new ConversationParticipant
            {
                ConversationId = conversationId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            })
            .ToList();

        await _conversationRepository.AddParticipantsAsync(participants);
    }

    public async Task<List<Conversation>> GetByUserIdAsync(int userId)
    {
        return await _conversationRepository.GetByUserIdAsync(userId);
    }

}
