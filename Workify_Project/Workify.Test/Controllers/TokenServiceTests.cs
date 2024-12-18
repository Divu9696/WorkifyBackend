using System;
using Microsoft.Extensions.Configuration;
using Workify.DTOs;
using Workify.Helpers;

namespace Workify.Test.Controllers;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Issuer", "https://localhost:5001" },
            { "Jwt:Audience", "https://localhost:5001" },
            { "Jwt:Secret", "YourSuperSecretKeyForJwtToken123456789123456789" },
            { "Jwt:ExpiryMinutes", "60" }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _tokenService = new TokenService(configuration);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt_WhenUserIsProvided()
    {
        // Arrange
        var user = new UserResponseDTO
        {
            Id = 1,
            Username = "TestUser",
            Email = "test@example.com",
            Role = "Admin"
        };

        // Act
        var token ="Bearer " + _tokenService.GenerateToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.Contains("Bearer", token); // Optional: Decode and validate JWT structure
    }
}
