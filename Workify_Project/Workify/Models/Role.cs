using System;

namespace Workify.Models;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = string.Empty; // E.g., "Employer", "JobSeeker", "Admin"
    public string Description { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    // public ICollection<User> Users { get; set; } = new List<User>();
}

