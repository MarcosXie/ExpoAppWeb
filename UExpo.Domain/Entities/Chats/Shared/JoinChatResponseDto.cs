namespace UExpo.Domain.Entities.Chats.Shared;

public class JoinChatResponseDto
{
	public Guid RoomId { get; set; }
	public List<BaseMessage> Messages { get; set; } = [];
}
