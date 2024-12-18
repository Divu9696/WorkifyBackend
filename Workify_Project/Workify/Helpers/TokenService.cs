using System;

namespace Workify.Helpers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Workify.DTOs;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(UserResponseDTO user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string RefreshToken(ClaimsPrincipal userPrincipal)
    {
        // Refresh token logic (can be extended for complex implementations)
        var userId = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = userPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = userPrincipal.FindFirstValue(ClaimTypes.Name);

        var refreshedUser = new UserResponseDTO
        {
            Id = int.Parse(userId),
            Username = username,
            Role = role
        };

        return GenerateToken(refreshedUser);
    }

    public void InvalidateToken(ClaimsPrincipal userPrincipal)
    {
        // Add logic to blacklist or invalidate tokens if required
    }
}
