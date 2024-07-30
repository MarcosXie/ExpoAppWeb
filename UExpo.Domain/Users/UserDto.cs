using System.ComponentModel.DataAnnotations;

namespace UExpo.Domain.Users;

//Data Transfer Object
public class UserDto
{
    [Required, MaxLength(50)]
    public string Name { get; set; } = null!;
    [Required, MaxLength(120)]
    public string Email { get; set; } = null!;
    [Required, MaxLength(150)]
    public string? Enterprise { get; set; }
    //[Required, Length(8, 8)]
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    [Required, MaxLength(30)]
    public string Country { get; set; } = null!;
}
