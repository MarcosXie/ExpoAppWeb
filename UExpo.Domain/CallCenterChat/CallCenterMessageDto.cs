namespace UExpo.Domain.CallCenterChat;

public class CallCenterMessageDto
{
    public Guid UserId { get; set; }
    public Guid SenderId { get; set; }
    public string SendedMessage { get; set; } = null!;
    public string TranslatedMessage { get; set; } = null!;
    public bool Readed { get; set; }
}
