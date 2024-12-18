using Moq;
using AutoMapper;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;
using Workify.Services;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workify.Test.Services
{
    public class JobListingServiceTests
    {
        private readonly Mock<IJobListingRepository> _jobListingRepositoryMock;
        private readonly Mock<ILogger<JobListingService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly JobListingService _jobListingService;

        public JobListingServiceTests()
        {
            _jobListingRepositoryMock = new Mock<IJobListingRepository>();
            _loggerMock = new Mock<ILogger<JobListingService>>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JobListing, JobListingResponseDTO>();
                cfg.CreateMap<JobListingDTO, JobListing>();
            });
            _mapper = configuration.CreateMapper();
            _jobListingService = new JobListingService(_jobListingRepositoryMock.Object, _mapper, _loggerMock.Object);
        }

        [Fact]
public async Task SearchJobsAsync_ShouldReturnEmpty_WhenNoJobsMatchCriteria()
{
    // Arrange
    var criteria = new JobSearchCriteriaDTO { Skills = "Python", MinSalary = 80000 };
    _jobListingRepositoryMock.Setup(repo => repo.SearchJobsAsync(criteria)).ReturnsAsync(new List<JobListing>());

    // Act
    var result = await _jobListingService.SearchJobsAsync(criteria);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}

[Fact]
public async Task GetJobListingsByEmployerIdAsync_ShouldReturnEmpty_WhenNoJobsExistForEmployer()
{
    // Arrange
    var employerId = 1;
    _jobListingRepositoryMock.Setup(repo => repo.GetJobListingsByEmployerIdAsync(employerId)).ReturnsAsync(new List<JobListing>());

    // Act
    var result = await _jobListingService.GetJobListingsByEmployerIdAsync(employerId);

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}
[Fact]
public async Task GetJobListingByIdAsync_ShouldReturnJobListing_WhenJobListingExists()
{
    // Arrange
    var jobListingId = 1;
    var jobListing = new JobListing { Id = jobListingId, Title = "Software Engineer" };
    _jobListingRepositoryMock.Setup(repo => repo.GetByIdAsync(jobListingId)).ReturnsAsync(jobListing);

    // Act
    var result = await _jobListingService.GetJobListingByIdAsync(jobListingId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Software Engineer", result.Title);
}



[Fact]
public async Task GetJobListingByIdAsync_ShouldReturnNull_WhenJobListingDoesNotExist()
{
    // Arrange
    var jobListingId = 1;
    _jobListingRepositoryMock.Setup(repo => repo.GetByIdAsync(jobListingId)).ReturnsAsync((JobListing)null);

    // Act
    var result = await _jobListingService.GetJobListingByIdAsync(jobListingId);

    // Assert
    Assert.Null(result);
}

[Fact]
public async Task UpdateJobListingAsync_ShouldUpdateJobListing_WhenJobListingExists()
{
    // Arrange
    var jobListingDto = new JobListingDTO { Title = "Updated Job Title" };
    var jobListing = new JobListing { Id = 1, Title = "Old Job Title" };
    _jobListingRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(jobListing);
    _jobListingRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<JobListing>())).Returns(Task.CompletedTask);

    // Act
    await _jobListingService.UpdateJobListingAsync(1, jobListingDto);

    // Assert
    Assert.Equal("Updated Job Title", jobListing.Title);
}

[Fact]
public async Task UpdateJobListingAsync_ShouldThrowKeyNotFoundException_WhenJobListingDoesNotExist()
{
    // Arrange
    var jobListingDto = new JobListingDTO { Title = "Updated Job Title" };
    _jobListingRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((JobListing)null);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobListingService.UpdateJobListingAsync(1, jobListingDto));
}

[Fact]
public async Task DeleteJobListingAsync_ShouldDeleteJobListing_WhenJobListingExists()
{
    // Arrange
    var jobListingId = 1;
    _jobListingRepositoryMock.Setup(repo => repo.DeleteAsync(jobListingId)).Returns(Task.CompletedTask);

    // Act
    await _jobListingService.DeleteJobListingAsync(jobListingId);

    // Assert
    _jobListingRepositoryMock.Verify(repo => repo.DeleteAsync(jobListingId), Times.Once);
}



    }

    

}
