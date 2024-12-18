using System;
using Workify.DTOs;
using Workify.Models;

namespace Workify.Services;

public interface IJobSeekerService
{
    Task<JobSeekerResponseDTO?> GetJobSeekerByIdAsync(int id);
    Task<JobSeekerResponseDTO?> GetJobSeekerByUserIdAsync(int userId);
    Task<IEnumerable<JobSeekerDTO>> GetAllJobSeekersAsync();
    Task<JobSeeker> AddJobSeekerAsync(JobSeekerDTO jobSeekerDto);
    Task UpdateJobSeekerAsync(int id, JobSeekerDTO jobSeekerDto);
    Task DeleteJobSeekerAsync(int id);
    Task<IEnumerable<RecomendationJobDto>> GetJobRecommendationsAsync(int jobSeekerId);
    Task<IEnumerable<JobSeekerDTO>> FilterCandidatesAsync(CandidateSearchCriteriaDTO criteria);
}
