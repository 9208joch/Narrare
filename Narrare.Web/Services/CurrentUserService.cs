using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class CurrentUserService
{
    public UserDto? CurrentUser { get; private set; }

    public bool IsLoggedIn => CurrentUser != null;

    public void Login(UserDto user)
    {
        CurrentUser = user;
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}