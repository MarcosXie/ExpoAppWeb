namespace UExpo.Domain.Authentication;

public class AuthenticatedUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Type { get; set; }
    public string? Email { get; set; }
}
