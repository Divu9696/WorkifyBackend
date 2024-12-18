using System;
using Workify.Models;

namespace Workify.Repository;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
}

