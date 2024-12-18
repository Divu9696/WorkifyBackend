using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.Services;

namespace Workify.Controllers
{
    [ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotificationsByUser(int userId)
    {
        try
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            _logger.LogInformation("Notifications retrieved successfully for UserId: {UserId}", userId);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving notifications for UserId: {UserId}", userId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [HttpGet("unread/{userId}")]
    // public async Task<IActionResult> GetUnreadNotifications(int userId)
    // {
    //     try
    //     {
    //         var unreadNotifications = await _notificationService.GetUnreadNotificationsByUserIdAsync(userId);
    //         _logger.LogInformation("Unread notifications retrieved successfully for UserId: {UserId}", userId);
    //         return Ok(unreadNotifications);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error occurred while retrieving unread notifications for UserId: {UserId}", userId);
    //         return StatusCode(500, "An error occurred while processing your request.");
    //     }
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _notificationService.MarkAsReadAsync(id);
            _logger.LogInformation("Notification marked as read for ID: {Id}", id);
            return Ok("Notification marked as read");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Notification not found for ID: {Id}", id);
            return NotFound("Notification not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while marking notification as read for ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(int id)
    {
        try
        {
            await _notificationService.DeleteNotificationAsync(id);
            _logger.LogInformation("Notification deleted successfully for ID: {Id}", id);
            return Ok("Notification deleted successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Notification not found for deletion with ID: {Id}", id);
            return NotFound("Notification not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting notification with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

}
