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
    public class JobSeekerServiceTests
    {
        private readonly Mock<IJobSeekerRepository> _jobSeekerRepositoryMock;
        private readonly Mock<IJobListingService> _jobListingServiceMock;
        private readonly IMapper _mapper;
        private readonly JobSeekerService _jobSeekerService;

        public JobSeekerServiceTests()
        {
            _jobSeekerRepositoryMock = new Mock<IJobSeekerRepository>();
            _jobListingServiceMock = new Mock<IJobListingService>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<JobSeeker, JobSeekerResponseDTO>();
                cfg.CreateMap<JobSeekerDTO, JobSeeker>();
                cfg.CreateMap<JobListing, JobListingDTO>();
            });
            _mapper = configuration.CreateMapper();
            _jobSeekerService = new JobSeekerService(_jobSeekerRepositoryMock.Object, _mapper, _jobListingServiceMock.Object);
        }

        [Fact]
public async Task GetJobSeekerByIdAsync_ShouldReturnJobSeeker_WhenJobSeekerExists()
{
    // Arrange
    var jobSeekerId = 1;
    var jobSeeker = new JobSeeker { Id = jobSeekerId, FullName = "John Doe", Skills = "C#, ASP.NET" };
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync(jobSeeker);

    // Act
    var result = await _jobSeekerService.GetJobSeekerByIdAsync(jobSeekerId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("John Doe", result.FullName);
    Assert.Equal("C#, ASP.NET", result.Skills);
}

[Fact]
public async Task GetJobSeekerByIdAsync_ShouldReturnNull_WhenJobSeekerDoesNotExist()
{
    // Arrange
    var jobSeekerId = 1;
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync((JobSeeker)null);

    // Act
    var result = await _jobSeekerService.GetJobSeekerByIdAsync(jobSeekerId);

    // Assert
    Assert.Null(result);
}
[Fact]
public async Task GetJobSeekerByUserIdAsync_ShouldReturnJobSeeker_WhenJobSeekerExists()
{
    // Arrange
    var userId = 1;
    var jobSeeker = new JobSeeker { Id = 1, UserId = userId, FullName = "John Doe", Skills = "C#, ASP.NET" };
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync(jobSeeker);

    // Act
    var result = await _jobSeekerService.GetJobSeekerByUserIdAsync(userId);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("John Doe", result.FullName);
}

[Fact]
public async Task GetJobSeekerByUserIdAsync_ShouldReturnNull_WhenJobSeekerDoesNotExist()
{
    // Arrange
    var userId = 1;
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByUserIdAsync(userId)).ReturnsAsync((JobSeeker)null);

    // Act
    var result = await _jobSeekerService.GetJobSeekerByUserIdAsync(userId);

    // Assert
    Assert.Null(result);
}


[Fact]
public async Task AddJobSeekerAsync_ShouldAddJobSeeker_WhenJobSeekerIsValid()
{
    // Arrange
    var jobSeekerDto = new JobSeekerDTO { FullName = "John Doe", Skills = "C#, ASP.NET" };
    var jobSeeker = new JobSeeker { Id = 1, FullName = "John Doe", Skills = "C#, ASP.NET" };
    _jobSeekerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<JobSeeker>())).Returns(Task.CompletedTask);

    // Act
    var result = await _jobSeekerService.AddJobSeekerAsync(jobSeekerDto);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("John Doe", result.FullName);
    Assert.Equal("C#, ASP.NET", result.Skills);
}
[Fact]
public async Task UpdateJobSeekerAsync_ShouldUpdateJobSeeker_WhenJobSeekerExists()
{
    // Arrange
    var jobSeekerId = 1;
    var jobSeekerDto = new JobSeekerDTO { FullName = "Updated Name", Skills = "C#, ASP.NET, SQL" };
    var jobSeeker = new JobSeeker { Id = jobSeekerId, FullName = "Old Name", Skills = "C#" };
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync(jobSeeker);
    _jobSeekerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<JobSeeker>())).Returns(Task.CompletedTask);

    // Act
    await _jobSeekerService.UpdateJobSeekerAsync(jobSeekerId, jobSeekerDto);

    // Assert
    Assert.Equal("Updated Name", jobSeeker.FullName);
    Assert.Equal("C#, ASP.NET, SQL", jobSeeker.Skills);
}

[Fact]
public async Task UpdateJobSeekerAsync_ShouldThrowKeyNotFoundException_WhenJobSeekerDoesNotExist()
{
    // Arrange
    var jobSeekerId = 1;
    var jobSeekerDto = new JobSeekerDTO { FullName = "Updated Name", Skills = "C#, ASP.NET" };
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync((JobSeeker)null);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobSeekerService.UpdateJobSeekerAsync(jobSeekerId, jobSeekerDto));
}
[Fact]
public async Task DeleteJobSeekerAsync_ShouldDeleteJobSeeker_WhenJobSeekerExists()
{
    // Arrange
    var jobSeekerId = 1;
    _jobSeekerRepositoryMock.Setup(repo => repo.DeleteAsync(jobSeekerId)).Returns(Task.CompletedTask);

    // Act
    await _jobSeekerService.DeleteJobSeekerAsync(jobSeekerId);

    // Assert
    _jobSeekerRepositoryMock.Verify(repo => repo.DeleteAsync(jobSeekerId), Times.Once);
}
[Fact]
public async Task GetJobRecommendationsAsync_ShouldReturnRecommendedJobs_WhenJobSeekerExists()
{
    // Arrange
    var jobSeekerId = 1;
    var jobSeeker = new JobSeeker { Id = jobSeekerId, Skills = "C#, ASP.NET" };
    var jobListings = new List<JobListingDTO>
    {
        new JobListingDTO { Title = "Software Engineer", Skills = "C#, ASP.NET" }
    };
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync(jobSeeker);
    _jobListingServiceMock.Setup(service => service.FilterJobsAsync(It.IsAny<JobSearchCriteriaDTO>())).ReturnsAsync(jobListings);

    // Act
    var result = await _jobSeekerService.GetJobRecommendationsAsync(jobSeekerId);

    // Assert
    Assert.NotNull(result);
    Assert.Single(result);
    Assert.Equal("Software Engineer", result.First().Title);
}

[Fact]
public async Task GetJobRecommendationsAsync_ShouldThrowKeyNotFoundException_WhenJobSeekerDoesNotExist()
{
    // Arrange
    var jobSeekerId = 1;
    _jobSeekerRepositoryMock.Setup(repo => repo.GetByIdAsync(jobSeekerId)).ReturnsAsync((JobSeeker)null);

    // Act & Assert
    await Assert.ThrowsAsync<KeyNotFoundException>(() => _jobSeekerService.GetJobRecommendationsAsync(jobSeekerId));
}



    }
}
