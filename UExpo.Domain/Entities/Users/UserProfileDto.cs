using Microsoft.AspNetCore.Http;

namespace UExpo.Domain.Entities.Users;

public class UserProfileDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string? Adress { get; set; }
    public string? Description { get; set; }

    public List<IFormFile> Images { get; set; } = [];
}
