using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Helpers;
using Workify.Services;

namespace Workify.Controllers
{
    [ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserService userService, ITokenService tokenService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        // _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDTO loginDto)
    {
        try
        {
            var user = await _userService.AuthenticateAsync(loginDto);
            if (user == null)
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", loginDto.Email);
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenService.GenerateToken(user);
            _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
            return Ok(new { Token = token, User = user });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for email: {Email}", loginDto.Email);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize]
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        try
        {
            var token = _tokenService.RefreshToken(User);
            _logger.LogInformation("Token refreshed for user: {UserId}", User.Identity?.Name);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during token refresh.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            _tokenService.InvalidateToken(User);
            _logger.LogInformation("User {UserId} logged out successfully.", User.Identity?.Name);
            return Ok("Logged out successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during logout.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}


}
