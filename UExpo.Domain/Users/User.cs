using UExpo.Domain.Shared;

namespace UExpo.Domain.Users;

public class User : BaseModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string Password { get; set; } = null!;
    public TypeEnum Type { get; set; }
    public string Country { get; set; } = null!;
}

