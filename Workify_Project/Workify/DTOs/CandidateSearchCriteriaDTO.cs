using System;

namespace Workify.DTOs;

public class CandidateSearchCriteriaDTO
{
    public string Skills { get; set; } = string.Empty; // Comma-separated skills
    public string Location { get; set; } = string.Empty;
    public string Education { get; set; } = string.Empty;
    public int MinExperience { get; set; }
}

