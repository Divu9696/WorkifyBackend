using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Controllers
{
    [ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO registerDto)
    {
        try
        {
            await _userService.RegisterAsync(registerDto);
            _logger.LogInformation("User registered successfully with email: {Email}", registerDto.Email);
            return Ok(new { message = "User registered successfully" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Registration failed for email: {Email}", registerDto.Email);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration for email: {Email}", registerDto.Email);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers(string role)
    {
        try
        {
            var users = await _userService.GetUsersByRoleAsync(role);
            _logger.LogInformation("Admin retrieved all users with role: {Role}", role);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving users.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Admin,User")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {Id}", id);
                return NotFound("User not found");
            }

            _logger.LogInformation("User details retrieved for ID: {Id}", id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, RegisterUserDTO userDto)
    {
        try
        {
            await _userService.UpdateUserAsync(id, userDto);
            _logger.LogInformation("User updated successfully for ID: {Id}", id);
            return Ok("User updated successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("User not found for update with ID: {Id}", id);
            return NotFound("User not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            _logger.LogInformation("User deleted successfully for ID: {Id}", id);
            return Ok("User deleted successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("User not found for deletion with ID: {Id}", id);
            return NotFound("User not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Admin")]
    // [HttpGet("analytics")]
    // public IActionResult GetUserAnalytics()
    // {
    //     try
    //     {
    //         var analytics = _userService.GetUserAnalytics();
    //         _logger.LogInformation("User analytics retrieved by Admin.");
    //         return Ok(analytics);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error occurred while retrieving user analytics.");
    //         return StatusCode(500, "An error occurred while processing your request.");
    //     }
    // }
}

}
