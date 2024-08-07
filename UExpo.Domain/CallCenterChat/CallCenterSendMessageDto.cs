namespace UExpo.Domain.CallCenterChat;

public class CallCenterSendMessageDto
{
    public Guid RoomId { get; set; }
    public Guid SenderId { get; set; }
    public string SendedMessage { get; set; } = null!;
}
