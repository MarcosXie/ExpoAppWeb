using System.ComponentModel.DataAnnotations;

namespace UExpo.Domain.Entities.Admins;

public class AdminDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;
}
