using Moq;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;
using Workify.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workify.Test.Services
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IApplicationRepository> _applicationRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ApplicationService _applicationService;

        public ApplicationServiceTests()
        {
            _applicationRepositoryMock = new Mock<IApplicationRepository>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Application, ApplicationResponseDTO>();
                cfg.CreateMap<ApplicationDTO, Application>();
            });
            _mapper = configuration.CreateMapper();
            _applicationService = new ApplicationService(_applicationRepositoryMock.Object, _mapper);
        }

        [Fact]
public async Task GetApplicationsByJobSeekerIdAsync_ShouldReturnApplications_WhenJobSeekerHasApplications()
{
    // Arrange
    var jobSeekerId = 1;
    var applications = new List<Application>
    {
        new Application { Id = 1, JobSeekerId = jobSeekerId, JobListingId = 1, Status = "Pending", AppliedAt = DateTime.UtcNow }
    };
    _applicationRepositoryMock.Setup(repo => repo.GetApplicationsByJobSeekerIdAsync(jobSeekerId)).ReturnsAsync(applications);

    // Act
    var result = await _applicationService.GetApplicationsByJobSeekerIdAsync(jobSeekerId);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal(jobSeekerId, result.First().JobSeekerId);
}

[Fact]
public async Task GetApplicationsByJobSeekerIdAsync_ShouldReturnEmpty_WhenJobSeekerHasNoApplications()
{
    // Arrange
    var jobSeekerId = 1;
    _applicationRepositoryMock.Setup(repo => repo.GetApplicationsByJobSeekerIdAsync(jobSeekerId)).ReturnsAsync(new List<Application>());

    // Act
    var result = await _applicationService.GetApplicationsByJobSeekerIdAsync(jobSeekerId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}

[Fact]
public async Task GetApplicationsByJobListingIdAsync_ShouldReturnApplications_WhenJobListingHasApplications()
{
    // Arrange
    var jobListingId = 1;
    var applications = new List<Application>
    {
        new Application { Id = 1, JobSeekerId = 1, JobListingId = jobListingId, Status = "Pending", AppliedAt = DateTime.UtcNow }
    };
    _applicationRepositoryMock.Setup(repo => repo.GetApplicationsByJobListingIdAsync(jobListingId)).ReturnsAsync(applications);

    // Act
    var result = await _applicationService.GetApplicationsByJobListingIdAsync(jobListingId);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal(jobListingId, result.First().JobListingId);
}

[Fact]
public async Task GetApplicationsByJobListingIdAsync_ShouldReturnEmpty_WhenJobListingHasNoApplications()
{
    // Arrange
    var jobListingId = 1;
    _applicationRepositoryMock.Setup(repo => repo.GetApplicationsByJobListingIdAsync(jobListingId)).ReturnsAsync(new List<Application>());

    // Act
    var result = await _applicationService.GetApplicationsByJobListingIdAsync(jobListingId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}
[Fact]
public async Task GetApplicationByIdAsync_ShouldReturnApplication_WhenApplicationExists()
{
    // Arrange
    var applicationId = 1;
    var application = new Application { Id = applicationId, JobSeekerId = 1, JobListingId = 1, Status = "Pending", AppliedAt = DateTime.UtcNow };
    _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ReturnsAsync(application);

    // Act
    var result = await _applicationService.GetApplicationByIdAsync(applicationId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(applicationId, result.Id);
}

[Fact]
public async Task GetApplicationByIdAsync_ShouldReturnNull_WhenApplicationDoesNotExist()
{
    // Arrange
    var applicationId = 1;
    _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ReturnsAsync((Application)null);

    // Act
    var result = await _applicationService.GetApplicationByIdAsync(applicationId);

    // Assert
    Assert.Null(result);
}
[Fact]
public async Task AddApplicationAsync_ShouldAddApplication_WhenApplicationIsValid()
{
    // Arrange
    var applicationDto = new ApplicationDTO { JobSeekerId = 1, JobListingId = 1 };
    var application = new Application { JobSeekerId = 1, JobListingId = 1, Status = "Pending", AppliedAt = DateTime.UtcNow };
    _applicationRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Application>())).Returns(Task.CompletedTask);

    // Act
    await _applicationService.AddApplicationAsync(applicationDto);

    // Assert
    _applicationRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Application>()), Times.Once);
}
[Fact]
public async Task UpdateApplicationStatusAsync_ShouldUpdateStatus_WhenApplicationExists()
{
    // Arrange
    var applicationId = 1;
    var status = "Accepted";
    var application = new Application { Id = applicationId, Status = "Pending" };
    _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ReturnsAsync(application);
    _applicationRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Application>())).Returns(Task.CompletedTask);

    // Act
    await _applicationService.UpdateApplicationStatusAsync(applicationId, status);

    // Assert
    Assert.Equal(status, application.Status);
    _applicationRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Application>()), Times.Once);
}

[Fact]
public async Task UpdateApplicationStatusAsync_ShouldThrowKeyNotFoundException_WhenApplicationDoesNotExist()
{
    // Arrange
    var applicationId = 1;
    var status = "Accepted";
    _applicationRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationId)).ReturnsAsync((Application)null);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _applicationService.UpdateApplicationStatusAsync(applicationId, status));
}
[Fact]
public async Task DeleteApplicationAsync_ShouldDeleteApplication_WhenApplicationExists()
{
    // Arrange
    var applicationId = 1;
    _applicationRepositoryMock.Setup(repo => repo.DeleteAsync(applicationId)).Returns(Task.CompletedTask);

    // Act
    await _applicationService.DeleteApplicationAsync(applicationId);

    // Assert
    _applicationRepositoryMock.Verify(repo => repo.DeleteAsync(applicationId), Times.Once);
}


    }
}
