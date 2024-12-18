using System;

namespace Workify.Models;

public class Employer
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyDescription { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();

    // Navigation Property
    public User User { get; set; } = null!;
    
}

