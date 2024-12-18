using System;
using System.ComponentModel.DataAnnotations;

namespace Workify.DTOs;

public class EmployerAllResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;
}
