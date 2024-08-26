namespace UExpo.Domain.Entities.CallCenterChat;

public class JoinChatResponseDto
{
    public List<CallCenterReceiveMessageDto> Messages { get; set; }
    public Guid RoomId { get; set; }
}
