using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Workify.DTOs;
using Workify.Services;

namespace Workify.Controllers
{
    
}
[ApiController]
[Route("api/[controller]")]
// [Authorize(Roles = "Admin,Employer")]
public class EmployerController : ControllerBase
{
    private readonly IEmployerService _employerService;
    private readonly ILogger<EmployerController> _logger;

    public EmployerController(IEmployerService employerService, ILogger<EmployerController> logger)
    {
        _employerService = employerService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployer(EmployerDTO employerDto)
    {
        try
        {
            var employerResponse = await _employerService.AddEmployerAsync(employerDto);
            //  var response= employerResponseDto;
            _logger.LogInformation("Employer added successfully for EmployerId: {UserId}", employerDto.UserId);
            // return Ok("Employer added successfully");
             return Ok(new { message = "Employer added successfully" , employerResponse});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding employer for Employer Id: {UserId}", employerDto.UserId);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployer(int id, EmployerDTO employerDto)
    {
        try
        {
            await _employerService.UpdateEmployerAsync(id, employerDto);
            _logger.LogInformation("Employer updated successfully for ID: {Id}", id);
            return Ok("Employer updated successfully");
        }
        catch (KeyNotFoundException)
        {
            _logger.LogWarning("Employer not found for update with ID: {Id}", id);
            return NotFound("Employer not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employer with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployerById(int id)
    {
        try
        {
            var employer = await _employerService.GetEmployerByIdAsync(id);
            if (employer == null)
            {
                _logger.LogWarning("Employer not found with ID: {Id}", id);
                return NotFound("Employer not found");
            }

            _logger.LogInformation("Employer details retrieved for ID: {Id}", id);
            return Ok(employer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving employer with ID: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    // EmployerController.cs
    [HttpGet("getEmployerIdByUserId/{userId}")]
    public async Task<IActionResult> GetEmployerIdByUserId(int userId)
    {
        var employer = await _employerService.GetEmployerByUserIdAsync(userId);
        if (employer == null)
        {
            return NotFound("Employer profile not found for the given UserId.");
        }

        return Ok(new { EmployerId = employer.Id });
    }


    [HttpGet]
    public async Task<IActionResult> GetAllEmployers()
    {
        try
        {
            var employers = await _employerService.GetAllEmployersAsync();
            _logger.LogInformation("All employers retrieved successfully.");
            return Ok(employers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all employers.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    // [HttpGet("analytics")]
    // public IActionResult GetJobPostingAnalytics()
    // {
    //     try
    //     {
    //         var analytics = _employerService.GetJobPostingAnalytics();
    //         _logger.LogInformation("Job posting analytics retrieved successfully.");
    //         return Ok(analytics);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Error occurred while retrieving job posting analytics.");
    //         return StatusCode(500, "An error occurred while processing your request.");
    //     }
    // }
}
