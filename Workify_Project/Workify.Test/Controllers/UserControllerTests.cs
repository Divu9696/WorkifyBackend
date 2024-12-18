using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Test.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        var loggerMock = new Mock<ILogger<UserController>>();
        _userController = new UserController(_userServiceMock.Object, loggerMock.Object);
        // _userServiceMock = new Mock<IUserService>();
        // _userController = new UserController(_userServiceMock.Object, null);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var mockUser = new UserResponseDTO { Id = 1, Username = "TestUser", Role = "Admin" };
        _userServiceMock.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync(mockUser);

        // Act
        var result = await _userController.GetUserById(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockUser, result?.Value);
    }

    [Fact]
    public async Task GetUserById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _userServiceMock.Setup(service => service.GetUserByIdAsync(1)).ReturnsAsync((UserResponseDTO?)null);

        // Act
        var result = await _userController.GetUserById(1) as NotFoundObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAllUsers_ShouldReturnListOfUsers_WhenRoleIsValid()
    {
        // Arrange
        var mockUsers = new List<UserResponseDTO>
        {
            new UserResponseDTO { Id = 1, Username = "Admin", Role = "Admin" },
            new UserResponseDTO { Id = 2, Username = "JobSeeker", Role = "JobSeeker" }
        };

        _userServiceMock.Setup(service => service.GetUsersByRoleAsync("Admin")).ReturnsAsync(mockUsers);

        // Act
        var result = await _userController.GetAllUsers("Admin") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mockUsers, result?.Value);
    }
}

