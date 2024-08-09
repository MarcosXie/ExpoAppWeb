using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Admins;

public class Admin : BaseModel, IChatUser
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;
    public string Language { get; set; } = "pt-BR";
}
