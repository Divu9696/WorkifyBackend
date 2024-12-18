using System;
using Workify.DTOs;
using Workify.Models;

namespace Workify.Services;

public interface IJobListingService
{
    Task<IEnumerable<JobListingResponseDTO>> SearchJobsAsync(JobSearchCriteriaDTO criteria);
    Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByEmployerIdAsync(int employerId);
    Task<JobListingResponseDTO?> GetJobListingByIdAsync(int id);
    Task<IEnumerable<JobListingResponseDTO>> GetAllJobListingsAsync();
    // Task<List<JobListing>> GetAllJobListingsAsync();
    // Task<List<JobListingResponseDTO>> GetAllJobListingsAsync();
    Task AddJobListingAsync(JobListingDTO jobListingDto);
    Task UpdateJobListingAsync(int id, JobListingDTO jobListingDto);
    Task DeleteJobListingAsync(int id);
    Task<IEnumerable<RecomendationJobDto>> FilterJobsAsync(JobSearchCriteriaDTO criteria);
    // Task<IEnumerable<JobListingDTO>> FilterJobsAsync(JobSearchCriteriaDTO criteria);
}

