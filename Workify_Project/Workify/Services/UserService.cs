using System;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Workify.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserResponseDTO?> AuthenticateAsync(LoginUserDTO loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);

        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return null; // Invalid credentials
        }

        return _mapper.Map<UserResponseDTO>(user);
    }

    public async Task RegisterAsync(RegisterUserDTO registerDto)
    {
        var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
        if (existingUser != null) throw new InvalidOperationException("Email is already registered");

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = HashPassword(registerDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        await _userRepository.AddAsync(user);
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserResponseDTO>(user);
    }

    public async Task<IEnumerable<UserResponseDTO>> GetUsersByRoleAsync(string role)
    {
        var users = await _userRepository.GetUsersByRoleAsync(role);
        return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
    }



    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    private bool VerifyPassword(string password, string hashedPassword)
    {
        // Implement password verification logic here
        // return password == hashedPassword; // Simplified
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }


    public async Task UpdateUserAsync(int id, RegisterUserDTO registerDto)
    {
        var employer = await _userRepository.GetByIdAsync(id);
        if (employer == null) throw new KeyNotFoundException("Employer not found");

        _mapper.Map(registerDto, employer);
        await _userRepository.UpdateAsync(employer);
    }
    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteAsync(id);
    }

}
