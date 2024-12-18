using System;

namespace Workify.Models;

public class JobListing
{
    public int Id { get; set; }
    public int EmployerId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty; // E.g., Full-time, Part-time
    public string Qualifications { get; set; } = string.Empty;
    public decimal Salary { get; set; }

    public string Skills { get; set; } = string.Empty ;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Application> Applications { get; set; } = new List<Application>();

    // Navigation Property
    public Employer Employer { get; set; } = null!;
}

