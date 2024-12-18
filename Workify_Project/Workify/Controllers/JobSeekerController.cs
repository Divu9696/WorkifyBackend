using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "Admin,JobSeeker")]
    public class JobSeekerController : ControllerBase
    {
        private readonly IJobSeekerService _jobSeekerService;
        private readonly ILogger<JobSeekerController> _logger;

        public JobSeekerController(IJobSeekerService jobSeekerService, ILogger<JobSeekerController> logger)
        {
            _jobSeekerService = jobSeekerService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddJobSeeker([FromForm] JobSeekerDTO jobSeekerDto, IFormFile resumeFile)
        {
            if (resumeFile == null || resumeFile.Length == 0)
            {
                return BadRequest("Resume file is required.");
            }
            try
            {
                // Define a path to save the file
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "resumes");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(resumeFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await resumeFile.CopyToAsync(stream);
                }

                // Set the ResumePath in the DTO
                jobSeekerDto.ResumePath = Path.Combine("resumes", uniqueFileName);

                var jobSeeker = await _jobSeekerService.AddJobSeekerAsync(jobSeekerDto);
                _logger.LogInformation("Job Seeker added successfully for UserId: {UserId}", jobSeekerDto.UserId);
                // return Ok("Job Seeker added successfully");
                return Ok(new 
                {
                    Message = "Job Seeker added successfully",
                    UserId = jobSeekerDto.UserId,
                    JobSeekerId = jobSeeker.Id
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding job seeker for UserId: {UserId}", jobSeekerDto.UserId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobSeeker(int id, JobSeekerDTO jobSeekerDto)
        {
            try
            {
                await _jobSeekerService.UpdateJobSeekerAsync(id, jobSeekerDto);
                _logger.LogInformation("Job Seeker updated successfully for ID: {Id}", id);
                return Ok("Job Seeker updated successfully");
            }
            catch (KeyNotFoundException)
            {
                _logger.LogWarning("Job Seeker not found for update with ID: {Id}", id);
                return NotFound("Job Seeker not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating job seeker with ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobSeekerById(int id)
        {
            try
            {
                var jobSeeker = await _jobSeekerService.GetJobSeekerByIdAsync(id);
                if (jobSeeker == null)
                {
                    _logger.LogWarning("Job Seeker not found with ID: {Id}", id);
                    return NotFound("Job Seeker not found");
                }

                _logger.LogInformation("Job Seeker details retrieved for ID: {Id}", id);
                return Ok(jobSeeker);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving job seeker with ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllJobSeekers()
        {
            try
            {
                var jobSeekers = await _jobSeekerService.GetAllJobSeekersAsync();
                _logger.LogInformation("All Job Seekers retrieved successfully.");
                return Ok(jobSeekers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all Job Seekers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("recommendations/{id}")]
        public async Task<IActionResult> GetJobRecommendations(int id)
        {
            try
            {
                var recommendations = await _jobSeekerService.GetJobRecommendationsAsync(id);
                _logger.LogInformation("Job recommendations retrieved for Job Seeker ID: {Id}", id);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving job recommendations for Job Seeker ID: {Id}", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

}
