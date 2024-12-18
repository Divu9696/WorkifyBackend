using System;
using System.ComponentModel.DataAnnotations;

namespace Workify.DTOs;

public class JobListingDTO
{
    public int EmployerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string JobType { get; set; } = string.Empty; // E.g., Full-Time, Part-Time

    public string Qualifications { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }
    // public string CompanyName { get; set; } = string.Empty;
}
