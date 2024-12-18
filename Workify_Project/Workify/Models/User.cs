using System;

namespace Workify.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Store hashed passwords
    public string Role { get; set; } = string.Empty; // "Employer" or "JobSeeker"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

