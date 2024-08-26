namespace UExpo.Domain.Entities.CallCenterChat;

public class CallCenterSendMessageDto
{
    public Guid RoomId { get; set; }
    public Guid SenderId { get; set; }
    public string SenderName { get; set; } = null!;
    public string SendedMessage { get; set; } = null!;
}
