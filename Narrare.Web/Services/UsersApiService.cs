using Narrare.Application.DTOs;

namespace Narrare.Web.Services;

public class UsersApiService
{
    private readonly ApiService _apiService;

    public UsersApiService(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<List<UserDto>?> GetAllAsync()
    {
        return await _apiService.GetAsync<List<UserDto>>(
            "api/Users");
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        return await _apiService.GetAsync<UserDto>(
            $"api/Users/{id}");
    }
    public async Task<UserDto?> RegisterAsync(RegisterUserDto user)
    {
        return await _apiService.PostAsync<RegisterUserDto, UserDto>(
            "api/Users/register",
            user);
    }
    public async Task<UserDto?> LoginAsync(LoginUserDto login)
    {
        return await _apiService.PostAsync<LoginUserDto, UserDto>(
            "api/Users/login",
            login);
    }
}