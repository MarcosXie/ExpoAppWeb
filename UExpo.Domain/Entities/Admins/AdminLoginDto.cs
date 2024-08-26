using System.ComponentModel.DataAnnotations;

namespace UExpo.Domain.Entities.Admins;

public class AdminLoginDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
