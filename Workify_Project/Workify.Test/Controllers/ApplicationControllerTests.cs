using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Workify.Controllers;
using Workify.DTOs;
using Workify.Services;
using Xunit;

public class ApplicationControllerTests
{
    private readonly Mock<IApplicationService> _applicationServiceMock;
    private readonly ApplicationController _controller;

    public ApplicationControllerTests()
    {
        _applicationServiceMock = new Mock<IApplicationService>();
        var loggerMock = new Mock<ILogger<ApplicationController>>(); // Mocking the logger (but not verifying it)

        _controller = new ApplicationController(_applicationServiceMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task SubmitApplication_ShouldReturnOk_WhenApplicationIsSubmittedSuccessfully()
    {
        // Arrange
        var applicationDto = new ApplicationDTO { JobSeekerId = 1, JobListingId = 1 };
        _applicationServiceMock.Setup(service => service.AddApplicationAsync(applicationDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SubmitApplication(applicationDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Application submitted successfully", okResult.Value);
    }

    [Fact]
    public async Task SubmitApplication_ShouldReturnInternalServerError_WhenAnErrorOccurs()
    {
        // Arrange
        var applicationDto = new ApplicationDTO { JobSeekerId = 1, JobListingId = 1 };
        _applicationServiceMock.Setup(service => service.AddApplicationAsync(applicationDto))
            .Throws(new Exception("Database error"));

        // Act
        var result = await _controller.SubmitApplication(applicationDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
    }

    

    [Fact]
    public async Task GetApplicationsByJobSeeker_ShouldReturnInternalServerError_WhenAnErrorOccurs()
    {
        // Arrange
        int jobSeekerId = 1;
        _applicationServiceMock.Setup(service => service.GetApplicationsByJobSeekerIdAsync(jobSeekerId))
            .Throws(new Exception("Database error"));

        // Act
        var result = await _controller.GetApplicationsByJobSeeker(jobSeekerId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
    }

    
    [Fact]
    public async Task UpdateApplicationStatus_ShouldReturnOk_WhenStatusIsUpdatedSuccessfully()
    {
        // Arrange
        int applicationId = 1;
        string status = "Accepted";
        _applicationServiceMock.Setup(service => service.UpdateApplicationStatusAsync(applicationId, status))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateApplicationStatus(applicationId, status);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Application status updated successfully", okResult.Value);
    }

    [Fact]
    public async Task UpdateApplicationStatus_ShouldReturnNotFound_WhenApplicationIsNotFound()
    {
        // Arrange
        int applicationId = 999;
        string status = "Rejected";
        _applicationServiceMock.Setup(service => service.UpdateApplicationStatusAsync(applicationId, status))
            .Throws(new KeyNotFoundException());

        // Act
        var result = await _controller.UpdateApplicationStatus(applicationId, status);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Application not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteApplication_ShouldReturnOk_WhenApplicationIsDeletedSuccessfully()
    {
        // Arrange
        int applicationId = 1;
        _applicationServiceMock.Setup(service => service.DeleteApplicationAsync(applicationId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteApplication(applicationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Application deleted successfully", okResult.Value);
    }

    [Fact]
    public async Task DeleteApplication_ShouldReturnNotFound_WhenApplicationIsNotFound()
    {
        // Arrange
        int applicationId = 999;
        _applicationServiceMock.Setup(service => service.DeleteApplicationAsync(applicationId))
            .Throws(new KeyNotFoundException());

        // Act
        var result = await _controller.DeleteApplication(applicationId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Application not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteApplication_ShouldReturnInternalServerError_WhenAnErrorOccurs()
    {
        // Arrange
        int applicationId = 1;
        _applicationServiceMock.Setup(service => service.DeleteApplicationAsync(applicationId))
            .Throws(new Exception("Database error"));

        // Act
        var result = await _controller.DeleteApplication(applicationId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
    }
}
