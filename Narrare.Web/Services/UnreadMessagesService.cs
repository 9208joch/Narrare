
namespace Narrare.Web.Services;

public class UnreadMessagesService
{
    public int UnreadPrivateConversationCount { get; private set; }

    public int UnreadGroupConversationCount { get; private set; }

    public int TotalUnreadConversationCount =>
        UnreadPrivateConversationCount + UnreadGroupConversationCount;

    public event Action? OnChange;

    public void SetUnreadPrivateConversationCount(int count)
    {
        UnreadPrivateConversationCount = Math.Max(0, count);
        OnChange?.Invoke();
    }

    public void SetUnreadGroupConversationCount(int count)
    {
        UnreadGroupConversationCount = Math.Max(0, count);
        OnChange?.Invoke();
    }

    public void Clear()
    {
        UnreadPrivateConversationCount = 0;
        UnreadGroupConversationCount = 0;
        OnChange?.Invoke();
    }
}
