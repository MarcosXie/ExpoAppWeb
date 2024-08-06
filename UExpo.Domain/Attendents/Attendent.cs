using UExpo.Domain.CallCenterChat;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Attendents;

public class Attendent : BaseModel, IChatUser
{
    public string Name { get; set; } = null!;
    public string Language { get; set; } = "en-US";
}
