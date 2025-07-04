using ExpoApp.Api.Hubs.Interfaces;
using ExpoShared.Domain.Entities.Chats.RelationshipChat;
using ExpoShared.Domain.Entities.Chats.Shared;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace ExpoApp.Api.Hubs;

public class RelationshipChatHub(
	IUserRepository userRepository,
	IRelationshipRepository relationshipRepository,
	IRelationshipChatService service, 
	IHubContext<NotificationsHub> notificationHub
) : BaseGoogleNotificationHub(userRepository, relationshipRepository), IChatHub
{
	public async Task<JoinChatResponseDto> JoinChatRoom(ChatDto joinChatDto)
	{
		var roomId = joinChatDto.Id;

		await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

		return new()
		{
			RoomId = roomId,
			Messages = await service.GetMessagesByChatAsync(joinChatDto)
		};
	}

	public async Task ChangeUserLang(ChatDto callCenterChat)
	{
		await service.UpdateLangAsync(callCenterChat);
	}

	public async Task SendMessage(SendMessageDto message)
	{
		ReceiveMessageDto msgDto = await service.AddMessageAsync(message);

		msgDto.SendedTime = msgDto.SendedTime.ToUniversalTime();

		await Clients.Group(msgDto.RoomId).SendAsync("ReceiveMessage", msgDto);

		await notificationHub.Clients.Groups(msgDto.ReceiverId.ToString()).SendAsync("Notification",
			new UserRoomNotificationsDto
			{
				RelationshipNotifications = await service.GetNotReadedMessagesAsync(msgDto.ReceiverId),
			});

		
		if (msgDto.SenderId != msgDto.ReceiverId)
		{
			await SendPushNotification(msgDto);
		}
	}

	public async Task VisualizeMessages(ChatDto chat)
	{
		await service.VisualizeMessagesAsync(chat);

		await Clients.Group(chat.Id.ToString()!).SendAsync("VisualizedMessages", chat.UserId);

		await notificationHub.Clients.Groups(chat.UserId.ToString()).SendAsync("Notification",
			new UserRoomNotificationsDto
			{
				RelationshipNotifications = await service.GetNotReadedMessagesAsync(chat.UserId),
			});
	}

	public async Task DeleteMessage(DeleteMsgDto deleteMsgDto)
	{
		await service.DeleteMessageAsync(deleteMsgDto);

		await Clients.Group(deleteMsgDto.RoomId.ToString()).SendAsync("DeleteMessage", deleteMsgDto);
	}
}
