using System;

namespace Workify.Models;

public class Application
{
    public int Id { get; set; }
    public int JobSeekerId { get; set; }
    public int JobListingId { get; set; }
    public string Status { get; set; } = "Pending"; // E.g., Pending, Accepted, Rejected
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public JobSeeker JobSeeker { get; set; } = null!;
    public JobListing JobListing { get; set; } = null!;
}

