using System;

namespace Workify.DTOs;

public class JobSearchCriteriaDTO
{
    public string Title { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public decimal? MinSalary { get; set; }
    public string JobType { get; set; } = string.Empty; // Full-Time, Part-Time
     public string Skills { get; set; } = string.Empty;

}
