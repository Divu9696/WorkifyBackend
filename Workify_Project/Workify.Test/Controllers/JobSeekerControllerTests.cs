using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Services;
using Xunit;

public class JobSeekerControllerTests
{
    private readonly Mock<IJobSeekerService> _jobSeekerServiceMock;
    private readonly JobSeekerController _controller;

    public JobSeekerControllerTests()
    {
        _jobSeekerServiceMock = new Mock<IJobSeekerService>();
        var loggerMock = new Mock<ILogger<JobSeekerController>>(); // Mocking the logger (but not verifying it)

        _controller = new JobSeekerController(_jobSeekerServiceMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task AddJobSeeker_ShouldReturnBadRequest_WhenResumeFileIsNull()
    {
        // Arrange
        var jobSeekerDto = new JobSeekerDTO { UserId = 1 };
        IFormFile resumeFile = null;

        // Act
        var result = await _controller.AddJobSeeker(jobSeekerDto, resumeFile);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Resume file is required.", badRequestResult.Value);
    }

    
    [Fact]
    public async Task UpdateJobSeeker_ShouldReturnOk_WhenJobSeekerIsUpdatedSuccessfully()
    {
        // Arrange
        var jobSeekerDto = new JobSeekerDTO { UserId = 1 };
        int jobSeekerId = 1;
        _jobSeekerServiceMock.Setup(service => service.UpdateJobSeekerAsync(jobSeekerId, jobSeekerDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateJobSeeker(jobSeekerId, jobSeekerDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Job Seeker updated successfully", okResult.Value);
    }

    [Fact]
    public async Task UpdateJobSeeker_ShouldReturnNotFound_WhenJobSeekerIsNotFound()
    {
        // Arrange
        var jobSeekerDto = new JobSeekerDTO { UserId = 1 };
        int jobSeekerId = 999;
        _jobSeekerServiceMock.Setup(service => service.UpdateJobSeekerAsync(jobSeekerId, jobSeekerDto))
            .Throws(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateJobSeeker(jobSeekerId, jobSeekerDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Job Seeker not found", notFoundResult.Value);
    }

    
    

    [Fact]
    public async Task GetAllJobSeekers_ShouldReturnOk_WhenJobSeekersAreFound()
    {
        // Arrange
        var jobSeekers = new List<JobSeekerDTO>
        {
            new JobSeekerDTO { Id = 1, UserId = 1 },
            new JobSeekerDTO { Id = 2, UserId = 2 }
        };
        _jobSeekerServiceMock.Setup(service => service.GetAllJobSeekersAsync())
            .ReturnsAsync(jobSeekers);

        // Act
        var result = await _controller.GetAllJobSeekers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<JobSeekerDTO>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetJobRecommendations_ShouldReturnOk_WhenRecommendationsAreFound()
    {
        // Arrange
        int jobSeekerId = 1;
        var recommendations = new List<JobListingDTO>
        {
            new JobListingDTO {  Title = "Software Engineer", EmployerId = 1 }
        };
        _jobSeekerServiceMock.Setup(service => service.GetJobRecommendationsAsync(jobSeekerId))
            .ReturnsAsync(recommendations);

        // Act
        var result = await _controller.GetJobRecommendations(jobSeekerId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<JobListingDTO>>(okResult.Value);
        Assert.Single(returnValue);
    }
}
