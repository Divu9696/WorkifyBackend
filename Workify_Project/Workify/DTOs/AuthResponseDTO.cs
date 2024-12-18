using System;

namespace Workify.DTOs;

public class AuthResponseDTO
{
    public string Token { get; set; }
    public UserResponseDTO User { get; set; }
}
