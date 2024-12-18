using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Helpers;
using Workify.Services;

namespace Workify.Test.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<AuthController>> _loggerMock;
    private readonly AuthController _authController;

   public AuthControllerTests()
    {
        // Create mocks for all dependencies
        _userServiceMock = new Mock<IUserService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<AuthController>>();

        // Pass mocked dependencies to the AuthController constructor
        _authController = new AuthController(
            _userServiceMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object
        );
    }

// [Fact]
// public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
// {
//     // Arrange
//     var loginDto = new LoginUserDTO
//     {
//         Email = "test@example.com",
//         Password = "Password123"
//     };

//     var user = new UserResponseDTO
//     {
//         Id = 1,
//         Username = "TestUser",
//         Email = "test@example.com",
//         Role = "Admin"
//     };

//     _userServiceMock.Setup(service => service.AuthenticateAsync(loginDto))
//         .ReturnsAsync(user);

//     _tokenServiceMock.Setup(service => service.GenerateToken(user))
//         .Returns("Bearer <token>");

//     // Act
//     var result = await _authController.Login(loginDto) as OkObjectResult;

//     // Debugging output
//     Console.WriteLine(result?.Value);  // Add this to check the result

//     // Assert
//     Assert.NotNull(result);
//     Assert.IsType<OkObjectResult>(result);
//     var response = result.Value as AuthResponseDTO;
//     Assert.NotNull(response);
//     Assert.NotNull(response.Token);
//     Assert.NotNull(response.User);
// }



    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var loginDto = new LoginUserDTO { Email = "test@example.com", Password = "WrongPassword" };

        _userServiceMock.Setup(service => service.AuthenticateAsync(loginDto)).ReturnsAsync((UserResponseDTO?)null);

        // Act
        var result = await _authController.Login(loginDto) as UnauthorizedObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    // [Fact]
    // public void Logout_ShouldReturnSuccess()
    // {
    //     // Act
    //     var result = _authController.Logout() as OkObjectResult;

    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.IsType<OkObjectResult>(result);
    // }
}
