using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.Models;

namespace Workify.Repository;

public class NotificationRepository : INotificationRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public NotificationRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await _dbContext.Notifications.ToListAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _dbContext.Notifications.FindAsync(id);
    }

    public async Task AddAsync(Notification notification)
    {
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Notification notification)
    {
        _dbContext.Notifications.Update(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var notification = await GetByIdAsync(id);
        if (notification != null)
        {
            _dbContext.Notifications.Remove(notification);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
    {
        return await _dbContext.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
