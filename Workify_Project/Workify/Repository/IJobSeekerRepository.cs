using System;
using Workify.Models;

namespace Workify.Repository;

public interface IJobSeekerRepository : IRepository<JobSeeker>
{
    Task<JobSeeker?> GetByUserIdAsync(int userId);
    IQueryable<JobSeeker> Query(); // For filtering
}

