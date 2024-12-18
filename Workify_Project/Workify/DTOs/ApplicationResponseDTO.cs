using System;

namespace Workify.DTOs;

public class ApplicationResponseDTO
{
    public int Id { get; set; }
    public int JobSeekerId { get; set; }
    public int JobListingId { get; set; }
    public string Status { get; set; } = "Pending"; // E.g., Pending, Accepted, Rejected
    public DateTime AppliedAt { get; set; }
}
