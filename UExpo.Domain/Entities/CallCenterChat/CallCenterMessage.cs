using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.CallCenterChat;

public class CallCenterMessage : BaseModel
{
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SenderLang { get; set; } = null!;
    public string SendedMessage { get; set; } = null!;
    public string ReceiverLang { get; set; } = null!;
    public string TranslatedMessage { get; set; } = null!;
    public bool Readed { get; set; }
}
