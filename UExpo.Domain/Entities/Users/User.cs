using UExpo.Domain.Entities.CallCenterChat;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Users;

public class User : BaseModel, IChatUser
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Enterprise { get; set; }
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsEmailValidated { get; set; } = false;
    public TypeEnum Type { get; set; }
    public string Language { get; set; } = "en-US";
    public string? Address { get; set; }
    public string? Description { get; set; }

    public List<UserImage> Images { get; set; } = [];
	public Catalog? Catalog { get; set; }
}

