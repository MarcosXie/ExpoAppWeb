using UExpo.Domain.Entities.Chats.RelationshipChat;

namespace UExpo.Domain.Entities.Chats.Shared;

public class UserRoomNotificationsDto
{
	public List<RelationshipNotReadedMessagesDto>? RelationshipNotifications { get; set; } = null;
	public int? CallCenterNotReadedMessages { get; set; } = null;
}

