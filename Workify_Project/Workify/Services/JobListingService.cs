using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class JobListingService : IJobListingService
{
    private readonly IJobListingRepository _jobListingRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<JobListingService> _logger;

    public JobListingService(IJobListingRepository jobListingRepository, IMapper mapper, ILogger<JobListingService> logger)
    {
        _jobListingRepository = jobListingRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<JobListingResponseDTO>> SearchJobsAsync(JobSearchCriteriaDTO criteria)
    {
        var jobs = await _jobListingRepository.SearchJobsAsync(criteria);
        return _mapper.Map<IEnumerable<JobListingResponseDTO>>(jobs);
    }

    public async Task<IEnumerable<JobListingResponseDTO>> GetJobListingsByEmployerIdAsync(int employerId)
    {
        var jobs = await _jobListingRepository.GetJobListingsByEmployerIdAsync(employerId);
        return _mapper.Map<IEnumerable<JobListingResponseDTO>>(jobs);
    }

    public async Task<JobListingResponseDTO?> GetJobListingByIdAsync(int id)
    {
        var jobListing = await _jobListingRepository.GetByIdAsync(id);
        return jobListing == null ? null : _mapper.Map<JobListingResponseDTO>(jobListing);
    }

    public async Task AddJobListingAsync(JobListingDTO jobListingDto)
    {
        var jobListing = _mapper.Map<JobListing>(jobListingDto);
        await _jobListingRepository.AddAsync(jobListing);
    }

    public async Task UpdateJobListingAsync(int id, JobListingDTO jobListingDto)
    {
        var jobListing = await _jobListingRepository.GetByIdAsync(id);
        if (jobListing == null) throw new KeyNotFoundException("Job listing not found");

        _mapper.Map(jobListingDto, jobListing);
        await _jobListingRepository.UpdateAsync(jobListing);
    }

    public async Task DeleteJobListingAsync(int id)
    {
        await _jobListingRepository.DeleteAsync(id);
    }


    public async Task<IEnumerable<RecomendationJobDto>> FilterJobsAsync(JobSearchCriteriaDTO criteria)
    {
        var jobsQuery = _jobListingRepository.Query();

        if (!string.IsNullOrEmpty(criteria.Skills))
        {
            var skillSet = criteria.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(skill => skill.Trim());

            jobsQuery = jobsQuery.Where(jl =>
                skillSet.Any(skill => jl.Skills.Contains(skill)));
        }

        // if (!string.IsNullOrEmpty(criteria.Location))
        // {
        //     jobsQuery = jobsQuery.Where(jl => jl.Location.Contains(criteria.Location));
        // }

        if (criteria.MinSalary > 0)
        {
            jobsQuery = jobsQuery.Where(jl => jl.Salary >= criteria.MinSalary);
        }

        if (!string.IsNullOrEmpty(criteria.JobType))
        {
            jobsQuery = jobsQuery.Where(jl => jl.JobType == criteria.JobType);
        }

        var filteredJobs = await jobsQuery.ToListAsync();
        return _mapper.Map<IEnumerable<RecomendationJobDto>>(filteredJobs);
    }

    public async Task<IEnumerable<JobListingResponseDTO>> GetAllJobListingsAsync()
    {
        // Retrieve all employers from the repository
        var joblistings = await _jobListingRepository.GetAllAsync();

        // Map entities to DTOs
        var jobListingsDTOs = _mapper.Map<IEnumerable<JobListingResponseDTO>>(joblistings);

        return jobListingsDTOs;
    }

}

