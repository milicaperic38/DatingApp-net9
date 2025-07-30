using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace API.DTOs;

public class RegisterDto
{
   

    [Required]
    public required string Password { get; set; }
    [Required]
    public required string Username { get; set; }
}