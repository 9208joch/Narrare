using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class CurrentUserService
{
    public UserDto? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser != null;

    public event Action? OnChange;

    public void Login(UserDto user)
    {
        CurrentUser = user;
        OnChange?.Invoke();
    }

    public void Logout()
    {
        CurrentUser = null;
        OnChange?.Invoke();
    }
}