using System;
using System.ComponentModel.DataAnnotations;

namespace Workify.DTOs;

public class JobSeekerDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string ContactNumber { get; set; } = string.Empty;

    public string Education { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string ResumePath { get; set; } = string.Empty; // Path to resume file
}

