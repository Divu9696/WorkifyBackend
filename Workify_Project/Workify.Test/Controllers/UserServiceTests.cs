using System;
using AutoMapper;
using Moq;
using Workify.DTOs;
using Workify.Mapping;
using Workify.Models;
using Workify.Repository;
using Workify.Services;

namespace Workify.Test.Controllers;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IMapper _mapperMock; // Use real AutoMapper configuration
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapperMock = config.CreateMapper();

        _userService = new UserService(_userRepositoryMock.Object, _mapperMock);
    }

    [Fact]
    public async Task RegisterAsync_ShouldRegisterUser_WhenEmailIsUnique()
    {
        // Arrange
        var registerDto = new RegisterUserDTO
        {
            Email = "test@example.com",
            Password = "Password123",
            Username = "TestUser",
            Role = "JobSeeker"
        };

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null); // No existing user

        // Act
        await _userService.RegisterAsync(registerDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var registerDto = new RegisterUserDTO
        {
            Email = "existing@example.com",
            Password = "Password123",
            Username = "TestUser",
            Role = "JobSeeker"
        };

        _userRepositoryMock.Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(new User { Email = "existing@example.com" });

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.RegisterAsync(registerDto));
    }
}
