using UExpo.Domain.Shared;

namespace UExpo.Domain.User;

public class User : BaseModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string? Password { get; set; }
    public string Type { get; set; } = null!;
    public string Country { get; set; } = null!;
}

