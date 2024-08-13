namespace UExpo.Domain.Authentication;

public class AuthenticatedUser
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public string? Email { get; set; }
}
