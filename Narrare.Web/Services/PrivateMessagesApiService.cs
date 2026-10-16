
using Narrare.Application.DTOs;

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
        return await _apiService.PutAsync<object, object>(
            $"api/PrivateMessages/{id}/read",
            new { }) is not null;
    }
}
