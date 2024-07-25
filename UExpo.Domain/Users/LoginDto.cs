using System.ComponentModel.DataAnnotations;

namespace UExpo.Domain.Users;

public class LoginDto
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
