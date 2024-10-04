namespace UExpo.Domain.Entities.Chats.Shared;

public class DeleteMsgDto
{
	public Guid RoomId { get; set; }
	public Guid MsgId { get; set; }
}
