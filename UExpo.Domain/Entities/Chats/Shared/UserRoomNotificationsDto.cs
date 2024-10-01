namespace UExpo.Domain.Entities.Chats.Shared;

public class UserRoomNotificationsDto
{
	public List<NotReadedMessagesDto>? RelationshipNotifications { get; set; } = null;
	public List<NotReadedMessagesDto>? CartNotifications { get; set; } = null;
	public int? CallCenterNotReadedMessages { get; set; } = null;
}

