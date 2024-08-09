using System.ComponentModel.DataAnnotations;

namespace UExpo.Domain.Admins;

public class AdminDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public AdminType Type { get; set; }
}
