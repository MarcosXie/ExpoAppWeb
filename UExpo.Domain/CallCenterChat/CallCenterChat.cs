using UExpo.Domain.Admins;
using UExpo.Domain.Shared;
using UExpo.Domain.Users;

namespace UExpo.Domain.CallCenterChat;

public class CallCenterChat : BaseModel
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid? AdminId { get; set; }
    public Admin? Admin { get; set; }
    public string UserLang { get; set; } = "en-US";
    public string AttendentLang { get; set; } = "pt-BR";

    public List<CallCenterMessage> Messages { get; set; } = [];
}
