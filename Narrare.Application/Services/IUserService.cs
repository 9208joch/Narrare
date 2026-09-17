using System;
using System.Collections.Generic;
using System.Text;
using Narrare.Domain.Entities;
using Narrare.Application.DTOs;

namespace Narrare.Application.Interfaces.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUsernameAsync(string username);

    Task<List<User>> GetAllAsync();

    Task<bool> UsernameExistsAsync(string username);

    Task<User?> RegisterAsync(RegisterUserDto dto);

    Task UpdateAsync(User user);

    Task DeleteAsync(int id);
}