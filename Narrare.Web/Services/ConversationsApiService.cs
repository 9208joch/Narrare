
using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class ConversationsApiService
{
    private readonly ApiService _apiService;

    public ConversationsApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<Conversation?> CreateGroupAsync(
    CreateGroupConversationDto request)
    {
        return await _apiService.PostAsync<CreateGroupConversationDto, Conversation>(
            "api/Conversations/group",
            request);
    }


    public async Task<bool> RenameAsync(
        int conversationId,
        RenameConversationDto request)
    {
        return await _apiService.PutWithoutResponseAsync(
            $"api/Conversations/{conversationId}/name",
            request);
    }

    public async Task<List<ConversationDto>?> GetMyConversationsAsync()
    {
        return await _apiService.GetAsync<List<ConversationDto>>(
            "api/Conversations");
    }

    
    public async Task<bool> AddParticipantsAsync(
        int conversationId,
        List<int> participantIds)
    {
        var request = new AddConversationParticipantsDto
        {
            ParticipantIds = participantIds
        };

        return await _apiService.PostAsync(
            $"api/Conversations/{conversationId}/participants",
            request);
    }


}
