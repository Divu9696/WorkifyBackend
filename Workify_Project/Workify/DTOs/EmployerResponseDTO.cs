using System;

namespace Workify.DTOs;

public class EmployerResponseDTO
{
    public int Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
}

