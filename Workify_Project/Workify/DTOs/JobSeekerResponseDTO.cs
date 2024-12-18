using System;

namespace Workify.DTOs;

public class JobSeekerResponseDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string ResumePath { get; set; } = string.Empty;
}

