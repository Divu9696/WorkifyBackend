using System;
using Workify.DTOs;

namespace Workify.Services;

public interface IUserService
{
    Task<UserResponseDTO?> AuthenticateAsync(LoginUserDTO loginDto);
    Task RegisterAsync(RegisterUserDTO registerDto);
    Task<UserResponseDTO?> GetUserByIdAsync(int id);
    Task<IEnumerable<UserResponseDTO>> GetUsersByRoleAsync(string role);
    Task UpdateUserAsync(int id, RegisterUserDTO userDto);
    Task DeleteUserAsync(int id);
}


