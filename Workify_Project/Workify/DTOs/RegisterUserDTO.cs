using System;
using System.ComponentModel.DataAnnotations;

namespace Workify.DTOs;

public class RegisterUserDTO
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty; // Employer, JobSeeker, Admin
}

