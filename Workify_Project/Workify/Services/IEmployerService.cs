using System;
using Workify.DTOs;

namespace Workify.Services;

public interface IEmployerService
{
    Task<EmployerResponseDTO?> GetEmployerByIdAsync(int id);
    Task<EmployerResponseDTO?> GetEmployerByUserIdAsync(int userId);
    Task<EmployerResponseDTO> AddEmployerAsync(EmployerDTO employerDto);
    Task UpdateEmployerAsync(int id, EmployerDTO employerDto);
    Task DeleteEmployerAsync(int id);
    Task<IEnumerable<EmployerAllResponseDto>> GetAllEmployersAsync();
}
