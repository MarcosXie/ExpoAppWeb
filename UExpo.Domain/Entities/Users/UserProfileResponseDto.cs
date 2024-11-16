namespace UExpo.Domain.Entities.Users;

public class UserProfileResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public required string Enterprise { get; set; }
    public string Name { get; set; } = null!;
    public required string Address { get; set; }
    public required string Country { get; set; }
    public required string Description { get; set; }

	public required string ProfileImageUri { get; set; }
	public List<string> Images { get; set; } = [];
}
