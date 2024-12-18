using System;

namespace Workify.Models;

public class JobSeeker
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string ResumePath { get; set; } = string.Empty; // Path to the uploaded resume
    public ICollection<Application> Applications { get; set; } = new List<Application>();

    // Navigation Property
    public User User { get; set; } = null!;
}

