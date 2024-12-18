using System;
using System.Security.Claims;
using Workify.DTOs;

namespace Workify.Helpers;

public interface ITokenService
{
    string GenerateToken(UserResponseDTO user);
    string RefreshToken(ClaimsPrincipal userPrincipal);
    void InvalidateToken(ClaimsPrincipal userPrincipal);
}
