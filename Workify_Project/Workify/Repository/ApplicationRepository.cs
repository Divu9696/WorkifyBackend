using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.Models;

namespace Workify.Repository;

public class ApplicationRepository : IApplicationRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public ApplicationRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Application>> GetAllAsync()
    {
        return await _dbContext.Applications.ToListAsync();
    }

    public async Task<Application?> GetByIdAsync(int id)
    {
        return await _dbContext.Applications.FindAsync(id);
    }

    public async Task AddAsync(Application application)
    {
        await _dbContext.Applications.AddAsync(application);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Application application)
    {
        _dbContext.Applications.Update(application);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var application = await GetByIdAsync(id);
        if (application != null)
        {
            _dbContext.Applications.Remove(application);
            await _dbContext.SaveChangesAsync();
        }
    }

    // public async Task<IEnumerable<Application>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId)
    // {
    //     return await _dbContext.Applications
    //         .Where(a => a.JobSeekerId == jobSeekerId)
    //         .Include(a => a.JobSeeker) 
    //         .ToListAsync();
    // }
    public async Task<IEnumerable<Application>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId)
{
    return await _dbContext.Applications
        .Where(a => a.JobSeekerId == jobSeekerId)
        .Include(a => a.JobSeeker)  // Ensure JobSeeker is included
        .ToListAsync();
}


    public async Task<IEnumerable<Application>> GetApplicationsByJobListingIdAsync(int jobListingId)
    {
        return await _dbContext.Applications
            .Where(a => a.JobListingId == jobListingId)
            .ToListAsync();
    }
}
