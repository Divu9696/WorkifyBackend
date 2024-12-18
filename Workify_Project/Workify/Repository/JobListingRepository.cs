using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.DTOs;
using Workify.Models;

namespace Workify.Repository;

public class JobListingRepository : IJobListingRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public JobListingRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<JobListing>> GetAllAsync()
    {
        return await _dbContext.JobListings
        .Include(j => j.Employer)
        .ToListAsync();
    }

    public async Task<List<JobListing>> GetAllJobListingsAsync()
    {
        return await _dbContext.JobListings
            .Include(j => j.Employer) // Include Employer details to get the company name
            .ToListAsync();
    }

    public async Task<JobListing?> GetByIdAsync(int id)
    {
        return await _dbContext.JobListings
            .Include(j => j.Employer)
            .FirstOrDefaultAsync(j => j.Id == id);
        // return await _dbContext.JobListings.FindAsync(id);
    }

    public async Task AddAsync(JobListing jobListing)
    {
        await _dbContext.JobListings.AddAsync(jobListing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobListing jobListing)
    {
        _dbContext.JobListings.Update(jobListing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobListing = await GetByIdAsync(id);
        if (jobListing != null)
        {
            _dbContext.JobListings.Remove(jobListing);
            await _dbContext.SaveChangesAsync();
        }
    }

    // public async Task<IEnumerable<JobListing>> SearchJobsAsync(JobSearchCriteriaDTO criteria)
    // {
    //     return await _dbContext.JobListings
    //         .Where(j => j.Title.Contains(criteria.Title) &&
    //                     j.Location == criteria.Location &&
    //                     j.Salary >= criteria.MinSalary &&
    //                     j.Skills == criteria.Skills &&
    //                     j.JobType == criteria.JobType)
    //                     .Include(j => j.Employer)
    //         .ToListAsync();
    // }
    public async Task<IEnumerable<JobListing>> SearchJobsAsync(JobSearchCriteriaDTO criteria)
    {
        var query = _dbContext.JobListings
            .Include(j => j.Employer) // Include Employer relationship
            .AsQueryable();

        // Apply filters dynamically based on the criteria
        if (!string.IsNullOrEmpty(criteria.Title))
        {
            query = query.Where(j => j.Title.Contains(criteria.Title));
        }

        if (!string.IsNullOrEmpty(criteria.Location))
        {
            query = query.Where(j => j.Location == criteria.Location);
        }

        if (criteria.MinSalary.HasValue)
        {
            query = query.Where(j => j.Salary >= criteria.MinSalary.Value);
        }

        if (!string.IsNullOrEmpty(criteria.Skills))
        {
            query = query.Where(j => j.Skills.Contains(criteria.Skills));
        }

        if (!string.IsNullOrEmpty(criteria.JobType))
        {
            query = query.Where(j => j.JobType == criteria.JobType);
        }

        return await query.ToListAsync();
    }


    public async Task<IEnumerable<JobListing>> GetJobListingsByEmployerIdAsync(int employerId)
    {
        return await _dbContext.JobListings
            .Where(j => j.EmployerId == employerId)
            .Include(j => j.Employer)
            .ToListAsync();
    }
    public IQueryable<JobListing> Query()
    {
        return _dbContext.JobListings.AsQueryable();
    }
}
