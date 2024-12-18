using System;
using Workify.Models;

namespace Workify.Repository;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
}

