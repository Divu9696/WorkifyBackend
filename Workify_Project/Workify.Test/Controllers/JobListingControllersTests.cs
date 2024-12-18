using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Services;
using Xunit;

public class JobListingControllerTests
{
    private readonly Mock<IJobListingService> _jobListingServiceMock;
    private readonly JobListingController _controller;

    public JobListingControllerTests()
    {
        _jobListingServiceMock = new Mock<IJobListingService>();
        var loggerMock = new Mock<ILogger<JobListingController>>(); // Mocking the logger (but not verifying it)

        _controller = new JobListingController(_jobListingServiceMock.Object, loggerMock.Object);
    }

    
    
    [Fact]
    public async Task AddJobListing_ShouldReturnOk_WhenJobListingIsAddedSuccessfully()
    {
        // Arrange
        var jobListingDto = new JobListingDTO { Title = "Software Engineer", EmployerId = 1 };
        _jobListingServiceMock.Setup(service => service.AddJobListingAsync(jobListingDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddJobListing(jobListingDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Job Listing added successfully", okResult.Value);
    }

    [Fact]
    public async Task UpdateJobListing_ShouldReturnOk_WhenJobListingIsUpdatedSuccessfully()
    {
        // Arrange
        var jobListingDto = new JobListingDTO { Title = "Software Engineer", EmployerId = 1 };
        int jobListingId = 1;
        _jobListingServiceMock.Setup(service => service.UpdateJobListingAsync(jobListingId, jobListingDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateJobListing(jobListingId, jobListingDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Job Listing updated successfully", okResult.Value);
    }

    [Fact]
    public async Task DeleteJobListing_ShouldReturnOk_WhenJobListingIsDeletedSuccessfully()
    {
        // Arrange
        int jobListingId = 1;
        _jobListingServiceMock.Setup(service => service.DeleteJobListingAsync(jobListingId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteJobListing(jobListingId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Job Listing deleted successfully", okResult.Value);
    }

    

    
}
