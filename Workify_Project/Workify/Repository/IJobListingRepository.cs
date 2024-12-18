using System;
using Workify.DTOs;
using Workify.Models;

namespace Workify.Repository;

public interface IJobListingRepository : IRepository<JobListing>
{
    Task<IEnumerable<JobListing>> SearchJobsAsync(JobSearchCriteriaDTO criteria);
    Task<IEnumerable<JobListing>> GetJobListingsByEmployerIdAsync(int employerId);
    IQueryable<JobListing> Query(); // For filtering
    Task<List<JobListing>> GetAllJobListingsAsync();
}
