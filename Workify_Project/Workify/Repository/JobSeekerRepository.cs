using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.Models;

namespace Workify.Repository;

public class JobSeekerRepository : IJobSeekerRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public JobSeekerRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<JobSeeker>> GetAllAsync()
    {
        return await _dbContext.JobSeekers.ToListAsync();
    }

    public async Task<JobSeeker?> GetByIdAsync(int id)
    {
        return await _dbContext.JobSeekers.FindAsync(id);
    }

    public async Task AddAsync(JobSeeker jobSeeker)
    {
        await _dbContext.JobSeekers.AddAsync(jobSeeker);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobSeeker jobSeeker)
    {
        _dbContext.JobSeekers.Update(jobSeeker);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobSeeker = await GetByIdAsync(id);
        if (jobSeeker != null)
        {
            _dbContext.JobSeekers.Remove(jobSeeker);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<JobSeeker?> GetByUserIdAsync(int userId)
    {
        return await _dbContext.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
    }

    public IQueryable<JobSeeker> Query()
    {
        return _dbContext.JobSeekers.AsQueryable();
    }
}

