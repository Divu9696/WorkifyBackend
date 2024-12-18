using System;
using AutoMapper;
using Moq;
using Workify.DTOs;
using Workify.Models;
using Workify.Repository;
using Workify.Services;

namespace Workify.Test.Controllers;

public class EmployerServiceTests
{
    private readonly Mock<IEmployerRepository> _employerRepositoryMock = new Mock<IEmployerRepository>();

    // private readonly Mock<IRepository<Employer>> _employerRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly EmployerService _employerService;

    public EmployerServiceTests()
    {
        _employerRepositoryMock = new Mock<IEmployerRepository>();
        // _employerRepositoryMock = new Mock<IRepository<Employer>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Employer, EmployerDTO>();
        });
        _mapperMock = config.CreateMapper();

        _employerService = new EmployerService(_employerRepositoryMock.Object, _mapperMock);
    }

    // [Fact]
// public async Task GetEmployerByIdAsync_ShouldReturnEmployer_WhenEmployerExists()
// {
//     // Arrange
//     var employer = new Employer { Id = 1, Name = "Test Employer" };
//     _employerRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(employer);

//     // Act
//     var result = await _employerService.GetEmployerByIdAsync(1);

//     // Assert
//     Assert.NotNull(result);
//     Assert.Equal("Test Employer", result.Name);
// }

[Fact]
public async Task GetEmployerByIdAsync_ShouldReturnNull_WhenEmployerDoesNotExist()
{
    // Arrange
    _employerRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Employer)null);

    // Act
    var result = await _employerService.GetEmployerByIdAsync(1);

    // Assert
    Assert.Null(result);
}

[Fact]
public async Task GetEmployerByUserIdAsync_ShouldReturnNull_WhenEmployerDoesNotExist()
{
    // Arrange
    _employerRepositoryMock.Setup(repo => repo.GetByUserIdAsync(It.IsAny<int>())).ReturnsAsync((Employer)null);

    // Act
    var result = await _employerService.GetEmployerByUserIdAsync(123);

    // Assert
    Assert.Null(result);
}
[Fact]
public async Task GetAllEmployersAsync_ShouldReturnEmpty_WhenNoEmployersExist()
{
    // Arrange
    _employerRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Employer>());

    // Act
    var result = await _employerService.GetAllEmployersAsync();

    // Assert
    Assert.NotNull(result);
    Assert.Empty(result);
}

[Fact]
public async Task DeleteEmployerAsync_ShouldDeleteEmployer_WhenEmployerExists()
{
    // Arrange
    _employerRepositoryMock.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

    // Act
    await _employerService.DeleteEmployerAsync(1);

    // Assert
    _employerRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
}


}

