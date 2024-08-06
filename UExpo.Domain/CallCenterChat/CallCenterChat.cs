using UExpo.Domain.Shared;

namespace UExpo.Domain.CallCenterChat;

public class CallCenterChat : BaseModel
{
    public Guid UserId { get; set; }
    public Guid? AttendentId { get; set; }
    public string UserLang { get; set; } = "en-US";
    public string AttendentLang { get; set; } = "pt-BR";

    public List<CallCenterMessage> Messages { get; set; } = [];
}
