using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.CartChat;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs;

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
