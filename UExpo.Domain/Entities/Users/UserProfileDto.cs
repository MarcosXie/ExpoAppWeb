namespace UExpo.Domain.Entities.Users;

public class UserProfileDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
}
