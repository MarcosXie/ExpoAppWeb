namespace UExpo.Domain.CallCenterChat;

public class CallCenterReceiveMessageDto
{
    public string RoomId { get; set; } = null!;
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SendedMessage { get; set; } = null!;
    public string? TranslatedMessage { get; set; } = null!;
    public DateTime SendedTime { get; set; }
    public bool Readed { get; set; }
}
