using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class JobListingController : ControllerBase
{
    private readonly IJobListingService _jobListingService;
    private readonly ILogger<JobListingController> _logger;

    public JobListingController(IJobListingService jobListingService, ILogger<JobListingController> logger)
    {
        _jobListingService = jobListingService;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        // _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobListings()
    {
        try
        {
            var jobListings = await _jobListingService.GetAllJobListingsAsync();
            _logger.LogInformation("All JobListings retrieved successfully.");
            return Ok(jobListings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all JobListings.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterJobs([FromQuery] JobSearchCriteriaDTO criteria)
    {
        try
        {
            var jobs = await _jobListingService.SearchJobsAsync(criteria);
            _logger.LogInformation("Jobs filtered successfully.");
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while filtering jobs.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> AddJobListing(JobListingDTO jobListingDto)
    {
        try
        {
            await _jobListingService.AddJobListingAsync(jobListingDto);
            _logger.LogInformation("Job Listing added successfully for EmployerId: {EmployerId}", jobListingDto.EmployerId);
            return Ok (new {message  = "Job Listing added successfully"});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding job listing for EmployerId: {EmployerId}", jobListingDto.EmployerId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Employer")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateJobListing(int id, JobListingDTO jobListingDto)
    {
        try
        {
            await _jobListingService.UpdateJobListingAsync(id, jobListingDto);
            _logger.LogInformation("Job Listing updated successfully for ID: {Id}", id);
            // return Ok("Job Listing updated successfully");
            return Ok(new { message = "Job Listing updated successfully" });
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Job Listing not found for update with ID: {Id}", id);
            return NotFound("Job Listing not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating job listing with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [Authorize(Roles = "Employer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobListing(int id)
    {
        try
        {
            await _jobListingService.DeleteJobListingAsync(id);
            _logger.LogInformation("Job Listing deleted successfully for ID: {Id}", id);
            return Ok("Job Listing deleted successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Job Listing not found for deletion with ID: {Id}", id);
            return NotFound("Job Listing not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting job listing with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobListingById(int id)
    {
        try
        {
            var jobListing = await _jobListingService.GetJobListingByIdAsync(id);
            if (jobListing == null)
            {
                _logger.LogWarning("Job Listing not found with ID: {Id}", id);
                return NotFound("Job Listing not found");
            }

            _logger.LogInformation("Job Listing details retrieved for ID: {Id}", id);
            // var jobListingDTO = _mapper.Map<JobListingDTO>(jobListing);
            return Ok(jobListing);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving job listing with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    [AllowAnonymous]
    // [Authorize(Roles = "Employer")]
    [HttpGet("employer/{employerId}")]
    public async Task<IActionResult> GetJobListingsByEmployer(int employerId)
    {
        try
        {
            var jobListings = await _jobListingService.GetJobListingsByEmployerIdAsync(employerId);
            _logger.LogInformation($"Fetching job listings for employer ID {employerId}");
            return Ok(jobListings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while fetching job listings for employer ID {employerId}");
            throw;
        }
    }

}

}
