using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs;

public class NotificationsHub(
	ICallCenterChatService callCenterService,
	IRelationshipChatService relationshipChatService) : Hub
{
	public async Task<UserRoomNotificationsDto> JoinUserNotificationRoom(Guid userId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

		var notifications = new UserRoomNotificationsDto()
		{
			CallCenterNotReadedMessages = await callCenterService.GetNotReadedMessagesByUserId(userId),
			RelationshipNotifications = await relationshipChatService.GetNotReadedMessagesAsync(userId)
		};

		return notifications;
	}
}
