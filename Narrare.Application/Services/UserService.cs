using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Application.Interfaces.Repositories;
using Narrare.Application.Interfaces.Services;
using Narrare.Domain.Entities;
using Narrare.Application.DTOs;

namespace Narrare.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }
    
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        return user != null;
    }

    public async Task<User?> RegisterAsync(RegisterUserDto dto)
    {
        if (await UsernameExistsAsync(dto.Username))
        {
            return null;
        }

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = dto.Password,
            Age = dto.Age,
            Location = dto.Location,
            Occupation = dto.Occupation,
            ProfileImageUrl = dto.ProfileImageUrl,
            CreatedAt = DateTime.UtcNow,
            IsBlocked = false
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return;
        }

        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
    }
}