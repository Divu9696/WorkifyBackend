using System;
using Workify.DTOs;

namespace Workify.Services;

public interface IApplicationService
{
    Task<IEnumerable<ApplicationResponseDTOResume>> GetApplicationsByJobSeekerIdAsync(int jobSeekerId);
    Task<IEnumerable<ApplicationResponseDTO>> GetApplicationsByJobListingIdAsync(int jobListingId);
    Task<ApplicationResponseDTO?> GetApplicationByIdAsync(int id);
    Task AddApplicationAsync(ApplicationDTO applicationDto);
    Task UpdateApplicationStatusAsync(int id, string status);
    Task DeleteApplicationAsync(int id);
}

