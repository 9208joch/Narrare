
using Narrare.Application.DTOs;
using Narrare.Domain.Entities;

namespace Narrare.Web.Services;

public class PrivateMessagesApiService
{
    private readonly ApiService _apiService;

    public PrivateMessagesApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    // Hämta inkorgen.
    public async Task<List<PrivateMessageDto>?> GetInboxAsync()
    {
        return await _apiService.GetAsync<List<PrivateMessageDto>>(
            "api/PrivateMessages/inbox");
    }

    // Hämta skickade meddelanden.
    public async Task<List<PrivateMessageDto>?> GetSentAsync()
    {
        return await _apiService.GetAsync<List<PrivateMessageDto>>(
            "api/PrivateMessages/sent");
    }

    public async Task<List<PrivateMessageDto>?> GetConversationAsync(
        int otherUserId)
    {
        return await _apiService.GetAsync<List<PrivateMessageDto>>(
            $"api/PrivateMessages/conversation/{otherUserId}");
    }


    // Skicka ett privat meddelande.
    public async Task<PrivateMessageDto?> CreateAsync(
        CreatePrivateMessageDto request)
    {
        return await _apiService.PostAsync<CreatePrivateMessageDto, PrivateMessageDto>(
            "api/PrivateMessages",
            request);
    }

    // Markera ett meddelande som läst.

    public async Task<bool> MarkAsReadAsync(int id)
    {
        return await _apiService.PutWithoutResponseAsync(
            $"api/PrivateMessages/{id}/read",
            new { });
    }


    public async Task<List<PrivateMessageDto>?> GetByConversationIdAsync(
        int conversationId)
    {
        return await _apiService.GetAsync<List<PrivateMessageDto>>(
            $"api/PrivateMessages/by-conversation/{conversationId}");
    }

    public async Task<PrivateMessageDto?> CreateGroupMessageAsync(
        CreateGroupMessageDto request)
    {
        return await _apiService.PostAsync<CreateGroupMessageDto, PrivateMessageDto>(
            "api/PrivateMessages/group",
            request);
    }


    public async Task<List<GroupMessageReadStatus>?>
        GetGroupReadStatusesAsync()
    {
        return await _apiService.GetAsync<List<GroupMessageReadStatus>>(
            "api/PrivateMessages/group-read-statuses");
    }

    public async Task<bool> MarkGroupConversationAsReadAsync(
        int conversationId)
    {
        return await _apiService.PutWithoutResponseAsync(
            $"api/PrivateMessages/by-conversation/{conversationId}/read",
            new { });
    }

}
