namespace UExpo.Domain.CallCenterChat;

public class JoinChatResponseDto
{
    public List<CallCenterReceiveMessageDto> Messages { get; set; }
    public Guid RoomId { get; set; }
}
