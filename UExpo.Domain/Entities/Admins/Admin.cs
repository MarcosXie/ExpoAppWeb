using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Admins;

public class Admin : BaseModel, IChatUser
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;
    public string Lang { get; set; } = "pt";
}
