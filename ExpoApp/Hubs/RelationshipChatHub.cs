using ExpoApp.Api.Hubs.Interfaces;
using ExpoShared.Domain.Entities.Chats.RelationshipChat;
using ExpoShared.Domain.Entities.Chats.Shared;
using ExpoShared.Domain.Entities.Users;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace ExpoApp.Api.Hubs;

public class RelationshipChatHub(
	IUserRepository userRepository,
	IRelationshipChatService service, 
	IHubContext<NotificationsHub> notificationHub
) : Hub, IChatHub
{
	private readonly string _relationshipNotificationRoom = "RelationshipNotificationRoom";

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

		await SendPushNotification(msgDto);
	}

	private async Task SendPushNotification(ReceiveMessageDto msgDto)
	{
		var receiver = await userRepository.GetByIdAsync(msgDto.ReceiverId);

		var message = new Message()
		{
			Notification = new Notification()
			{
				Title = msgDto.SenderName,
				Body = msgDto.SendedMessage
			},
			Data = new Dictionary<string, string>()
			{
				{ "roomId", msgDto.RoomId },
				{ "senderId", msgDto.SenderId.ToString() },
				{ "message", msgDto.SendedMessage },
				{ "profileImage", receiver.ProfileImageUri ?? ""}
			},
			Token = receiver.FcmToken
		};

		try
		{
			string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
			Console.WriteLine($"Successfully sent push notification: {response}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error sending push notification: {ex.Message}");
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
