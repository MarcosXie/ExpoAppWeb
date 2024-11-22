using ExpoShared.Domain.Entities.Chats.CallCenterChat;
using ExpoShared.Domain.Entities.Chats.CartChat;
using ExpoShared.Domain.Entities.Chats.RelationshipChat;
using ExpoShared.Domain.Entities.Chats.Shared;
using Microsoft.AspNetCore.SignalR;

namespace ExpoApp.Api.Hubs;

public class NotificationsHub(
	ICallCenterChatService callCenterService,
	IRelationshipChatService relationshipChatService,
	ICartChatService cartChatService
) : Hub
{
	public async Task<UserRoomNotificationsDto> JoinUserNotificationRoom(Guid userId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());

		var notifications = new UserRoomNotificationsDto()
		{
			CallCenterNotReadedMessages = await callCenterService.GetNotReadedMessagesByUserId(userId),
			RelationshipNotifications = await relationshipChatService.GetNotReadedMessagesAsync(userId),
			CartNotifications = await cartChatService.GetNotReadedMessagesAsync(userId)
		};

		return notifications;
	}
}
