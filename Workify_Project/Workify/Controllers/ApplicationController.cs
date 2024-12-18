using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Controllers
{
    [ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin,Employer,JobSeeker")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationController> _logger;

    public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    [HttpPost]
    // [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> SubmitApplication(ApplicationDTO applicationDto)
    {
        try
        {
            await _applicationService.AddApplicationAsync(applicationDto);
            _logger.LogInformation("Application submitted successfully for JobSeekerId: {JobSeekerId} and JobListingId: {JobListingId}", applicationDto.JobSeekerId, applicationDto.JobListingId);
            return Ok(new { message = "Application submitted successfully" });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while submitting application for JobSeekerId: {JobSeekerId}", applicationDto.JobSeekerId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("jobseeker/{jobSeekerId}")]
    // [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> GetApplicationsByJobSeeker(int jobSeekerId)
    {
        try
        {
            var applications = await _applicationService.GetApplicationsByJobSeekerIdAsync(jobSeekerId);
            _logger.LogInformation("Applications retrieved successfully for JobSeekerId: {JobSeekerId}", jobSeekerId);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving applications for JobSeekerId: {JobSeekerId}", jobSeekerId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("joblisting/{jobListingId}")]
    // [Authorize(Roles = "Employer")]
    public async Task<IActionResult> GetApplicationsByJobListing(int jobListingId)
    {
        try
        {
            var applications = await _applicationService.GetApplicationsByJobListingIdAsync(jobListingId);
            _logger.LogInformation("Applications retrieved successfully for JobListingId: {JobListingId}", jobListingId);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving applications for JobListingId: {JobListingId}", jobListingId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{id}")]
    // [Authorize(Roles = "Employer")]
    public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] StatusUpdateDto statusUpdateDto)
    {
        try
        {
            await _applicationService.UpdateApplicationStatusAsync(id, statusUpdateDto.Status);
            _logger.LogInformation("Application status updated successfully for ID: {Id} to {Status}", id, statusUpdateDto.Status);
            return Ok("Application status updated successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Application not found for ID: {Id}", id);
            return NotFound("Application not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating application status for ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteApplication(int id)
    {
        try
        {
            await _applicationService.DeleteApplicationAsync(id);
            _logger.LogInformation("Application deleted successfully for ID: {Id}", id);
            return Ok("Application deleted successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Application not found for deletion with ID: {Id}", id);
            return NotFound("Application not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting application with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}

}
