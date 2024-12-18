using System;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NotificationResponseDTO>> GetNotificationsByUserIdAsync(int userId)
    {
        var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<NotificationResponseDTO>>(notifications);
    }

    public async Task AddNotificationAsync(NotificationDTO notificationDto)
    {
        var notification = _mapper.Map<Notification>(notificationDto);
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;
        await _notificationRepository.AddAsync(notification);
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null) throw new KeyNotFoundException("Notification not found");

        notification.IsRead = true;
        await _notificationRepository.UpdateAsync(notification);
    }

    public async Task DeleteNotificationAsync(int id)
    {
        await _notificationRepository.DeleteAsync(id);
    }
}

