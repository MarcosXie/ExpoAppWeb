namespace UExpo.Domain.Entities.Users;

public class UserProfileResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Company { get; set; }
    public string Name { get; set; } = null!;
    public string? Adress { get; set; }
    public string Country { get; set; } = null!;
    public string? Description { get; set; }
    public string Password { get; set; } = null!;

    public List<string> Images { get; set; } = [];
}
