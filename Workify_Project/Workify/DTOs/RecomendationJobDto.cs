using System;

namespace Workify.DTOs;

public class RecomendationJobDto
{
    public int Id { get; set; }           // Add this line to include the JobListing ID
    public int EmployerId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string JobType { get; set; }
    public string Qualifications { get; set; }
    public string Skills { get; set; }
    public decimal Salary { get; set; }
}

