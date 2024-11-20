using Microsoft.AspNetCore.SignalR;
using UExpo.Api.Hubs.Interfaces;
using UExpo.Domain.Entities.Chats.CartChat;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs;

public class CartChatHub(ICartChatService service, IHubContext<NotificationsHub> notificationHub) : Hub, IChatHub
{
	private readonly string _relationshipNotificationRoom = "CartNotificationRoom";

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
				CartNotifications = await service.GetNotReadedMessagesAsync(msgDto.ReceiverId),
			});
	}

	public async Task VisualizeMessages(ChatDto chat)
	{
		await service.VisualizeMessagesAsync(chat);

		await Clients.Group(chat.Id.ToString()!).SendAsync("VisualizedMessages", chat.UserId);

		await notificationHub.Clients.Groups(chat.UserId.ToString()).SendAsync("Notification",
			new UserRoomNotificationsDto
			{
				CartNotifications = await service.GetNotReadedMessagesAsync(chat.UserId),
			});
	}

	public async Task DeleteMessage(DeleteMsgDto message)
	{
		await service.DeleteMessageAsync(message);

		await Clients.Group(message.RoomId.ToString()).SendAsync("DeleteMessage", message);
	}
}
