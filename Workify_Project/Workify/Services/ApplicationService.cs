using System;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;

namespace Workify.Services;

public class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IMapper _mapper;
    private readonly IJobSeekerRepository _jobSeekerRepository;

    public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper, IJobSeekerRepository jobSeekerRepository)
    {
        _applicationRepository = applicationRepository;
        _mapper = mapper;
        _jobSeekerRepository = jobSeekerRepository;
    }

    public async Task<IEnumerable<ApplicationResponseDTOResume>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId)
    {
        var applications = await _applicationRepository.GetApplicationsByJobSeekerIdAsync(jobSeekerId);

        return _mapper.Map<IEnumerable<ApplicationResponseDTOResume>>(applications);
    }

    public async Task<IEnumerable<ApplicationResponseDTO>> GetApplicationsByJobListingIdAsync(int jobListingId)
    {
        var applications = await _applicationRepository.GetApplicationsByJobListingIdAsync(jobListingId);
        return _mapper.Map<IEnumerable<ApplicationResponseDTO>>(applications);
    }

    public async Task<ApplicationResponseDTO?> GetApplicationByIdAsync(int id)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        return application == null ? null : _mapper.Map<ApplicationResponseDTO>(application);
    }

    public async Task AddApplicationAsync(ApplicationDTO applicationDto)
    {
        var application = _mapper.Map<Application>(applicationDto);
        application.Status = "Pending";
        application.AppliedAt = DateTime.UtcNow;
        await _applicationRepository.AddAsync(application);
    }

    public async Task UpdateApplicationStatusAsync(int id, string status)
    {
        var application = await _applicationRepository.GetByIdAsync(id);
        if (application == null) throw new KeyNotFoundException("Application not found");

        application.Status = status;
        await _applicationRepository.UpdateAsync(application);
    }

    public async Task DeleteApplicationAsync(int id)
    {
        await _applicationRepository.DeleteAsync(id);
    }
}

