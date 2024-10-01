namespace UExpo.Domain.Entities.Chats.Shared;

public class NotReadedMessagesDto
{
	public Guid ChatId { get; set; }
	public int NotReadedMessages { get; set; }
}
