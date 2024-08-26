using Microsoft.AspNetCore.Http;

namespace UExpo.Domain.Entities.Users;

public class UserProfileDto
{
    public string Email { get; set; } = null!;
    public string? Company { get; set; }
    public string Name { get; set; } = null!;
    public string? Adress { get; set; }
    public string Country { get; set; } = null!;
    public string? Description { get; set; }
    public string Password { get; set; } = null!;

    public List<IFormFile> Images { get; set; } = [];
}
