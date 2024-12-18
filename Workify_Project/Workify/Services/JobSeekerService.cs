using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class JobSeekerService : IJobSeekerService
{
    private readonly IJobSeekerRepository _jobSeekerRepository;
    private readonly IJobListingService _jobListingService;
    private readonly IMapper _mapper;

    public JobSeekerService(IJobSeekerRepository jobSeekerRepository, IMapper mapper,IJobListingService jobListingService)
    {
        _jobSeekerRepository = jobSeekerRepository;
        _mapper = mapper;
        _jobListingService = jobListingService;

    }

    public async Task<JobSeekerResponseDTO?> GetJobSeekerByIdAsync(int id)
    {
        var jobSeeker = await _jobSeekerRepository.GetByIdAsync(id);
        return jobSeeker == null ? null : _mapper.Map<JobSeekerResponseDTO>(jobSeeker);
    }

    public async Task<JobSeekerResponseDTO?> GetJobSeekerByUserIdAsync(int userId)
    {
        var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
        return jobSeeker == null ? null : _mapper.Map<JobSeekerResponseDTO>(jobSeeker);
    }
    public async Task<IEnumerable<JobSeekerDTO>> GetAllJobSeekersAsync()
    {
        // Retrieve all employers from the repository
        var jobseekers = await _jobSeekerRepository.GetAllAsync();

        // Map entities to DTOs
        var job = _mapper.Map<IEnumerable<JobSeekerDTO>>(jobseekers);

        return job;
    }


    public async Task <JobSeeker> AddJobSeekerAsync(JobSeekerDTO jobSeekerDto)
    {
        var jobSeeker = _mapper.Map<JobSeeker>(jobSeekerDto);
        await _jobSeekerRepository.AddAsync(jobSeeker);
        return jobSeeker;
    }

    public async Task UpdateJobSeekerAsync(int id, JobSeekerDTO jobSeekerDto)
    {
        var jobSeeker = await _jobSeekerRepository.GetByIdAsync(id);
        if (jobSeeker == null) throw new KeyNotFoundException("Job Seeker not found");

        _mapper.Map(jobSeekerDto, jobSeeker);
        await _jobSeekerRepository.UpdateAsync(jobSeeker);
    }

    public async Task DeleteJobSeekerAsync(int id)
    {
        await _jobSeekerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<JobSeekerDTO>> FilterCandidatesAsync(CandidateSearchCriteriaDTO criteria)
    {
        var candidatesQuery = _jobSeekerRepository.Query();

        // Apply filters
        if (!string.IsNullOrEmpty(criteria.Skills))
        {
            var skillSet = criteria.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(skill => skill.Trim());
            candidatesQuery = candidatesQuery.Where(js => 
                skillSet.Any(skill => js.Skills.Contains(skill)));
        }

        if (!string.IsNullOrEmpty(criteria.Location))
        {
            candidatesQuery = candidatesQuery.Where(js => js.ContactNumber.Contains(criteria.Location)); // Assuming location
        }

        if (!string.IsNullOrEmpty(criteria.Education))
        {
            candidatesQuery = candidatesQuery.Where(js => js.Education.Contains(criteria.Education));
        }

        // if (criteria.MinExperience > 0)
        // {
        //     candidatesQuery = candidatesQuery.Where(js => js.ExperienceYears >= criteria.MinExperience);
        // }

        var filteredCandidates = await candidatesQuery.ToListAsync();
        return _mapper.Map<IEnumerable<JobSeekerDTO>>(filteredCandidates);
    }

    public async Task<IEnumerable<RecomendationJobDto>> GetJobRecommendationsAsync(int jobSeekerId)
    {
        // Retrieve the JobSeeker profile
        var jobSeeker = await _jobSeekerRepository.GetByIdAsync(jobSeekerId);
        if (jobSeeker == null)
        {
            throw new KeyNotFoundException($"JobSeeker with ID {jobSeekerId} not found.");
        }

        // Define recommendation criteria based on job seeker skills and location
        var criteria = new JobSearchCriteriaDTO
        {
            Skills = jobSeeker.Skills,          // Use the job seeker's skills
            // Location = jobSeeker.Location,     // Ensure 'Location' exists in the JobSeeker model
            // MinSalary = 0,                     // Optional: Default or derived value
            // JobType = string.Empty  // Assuming location is stored here
        };

        // Get recommended jobs using the JobListingService
        var recommendedJobs = await _jobListingService.FilterJobsAsync(criteria);

        return recommendedJobs;
    }
}

