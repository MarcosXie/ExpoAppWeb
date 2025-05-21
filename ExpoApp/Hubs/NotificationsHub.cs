using ExpoShared.Domain.Entities.Chats.CallCenterChat;
using ExpoShared.Domain.Entities.Chats.CartChat;
using ExpoShared.Domain.Entities.Chats.GroupChat;
using ExpoShared.Domain.Entities.Chats.RelationshipChat;
using ExpoShared.Domain.Entities.Chats.Shared;
using ExpoShared.Domain.Entities.Groups;
using Microsoft.AspNetCore.SignalR;

namespace ExpoApp.Api.Hubs;

public class NotificationsHub(
	ICallCenterChatService callCenterService,
	IRelationshipChatService relationshipChatService,
	IGroupChatService groupService,
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
			GroupNotifications = await groupService.GetNotReadedMessagesAsync(userId),
			CartNotifications = await cartChatService.GetNotReadedMessagesAsync(userId),
			NewCarts = await cartChatService.GetNewCartsAsync(userId),
		};

		return notifications;
	}
}
