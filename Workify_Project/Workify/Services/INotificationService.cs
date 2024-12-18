using System;
using Workify.DTOs;

namespace Workify.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDTO>> GetNotificationsByUserIdAsync(int userId);
    Task AddNotificationAsync(NotificationDTO notificationDto);
    Task MarkAsReadAsync(int notificationId);
    Task DeleteNotificationAsync(int id);
}

