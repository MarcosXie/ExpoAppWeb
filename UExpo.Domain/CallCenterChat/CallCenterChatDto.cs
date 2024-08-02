namespace UExpo.Domain.CallCenterChat;

public class CallCenterChatDto
{
    public Guid UserId { get; set; }
    public Guid AttendentId { get; set; }
    public string UserLang { get; set; } = "en-US";
    public string AttendentLang { get; set; } = "pt-BR";
}
