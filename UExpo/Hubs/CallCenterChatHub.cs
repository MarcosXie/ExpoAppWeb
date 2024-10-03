using Microsoft.AspNetCore.SignalR;
using UExpo.Api.Hubs.Interfaces;
using UExpo.Domain.Entities.Chats.CallCenterChat;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs;

public class CallCenterChatHub(ICallCenterChatService service, IHubContext<NotificationsHub> notificationHub) : Hub, IChatHub
{
	private readonly string _adminNotificationRoom = "AdminRoom";

	public async Task<JoinChatResponseDto> JoinChatRoom(ChatDto callCenterChat)
	{
		Guid roomId = await service.CreateCallCenterChatAsync(callCenterChat);

		await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

		callCenterChat.Id = roomId;

		return new()
		{
			RoomId = roomId,
			Messages = await service.GetMessagesByChatAsync(callCenterChat),
		};
	}

	public async Task ChangeUserLang(ChatDto callCenterChat)
	{
		await service.UpdateChatAsync(callCenterChat);
	}

	public async Task<List<CallCenterChatResponseDto>> JoinAdminNotificationRoom()
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, _adminNotificationRoom);
		return await service.GetChatsAsync();
	}

	public async Task<int> JoinUserNotificationRoom(Guid userId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
		return await service.GetNotReadedMessagesByUserId(userId);
	}

	public async Task SendMessage(SendMessageDto message)
	{
		(ReceiveMessageDto msgDto, bool isSendedByUser) = await service.AddMessageAsync(message);

		await Clients.Group(msgDto.RoomId).SendAsync("ReceiveMessage", msgDto);

		if (isSendedByUser)
		{
			await Clients.Groups(_adminNotificationRoom).SendAsync("UpdatedChats", await service.GetChatsAsync());
		}
		else
		{// User Notification Room
			await notificationHub.Clients.Groups(msgDto.ReceiverId.ToString()).SendAsync("Notification",
				new UserRoomNotificationsDto
				{
					CallCenterNotReadedMessages = await service.GetNotReadedMessagesByUserId(msgDto.ReceiverId),
				}
			);
		}
	}

	public async Task VisualizeMessages(ChatDto callCenterChat)
	{
		await service.VisualizeMessagesAsync(callCenterChat);

		await Clients.Group(callCenterChat.Id.ToString()!).SendAsync("VisualizedMessages", callCenterChat.UserId);

		await notificationHub.Clients.Groups(callCenterChat.UserId.ToString()).SendAsync("Notification",
			new UserRoomNotificationsDto
			{
				CallCenterNotReadedMessages = 0,
			}
		);
	}

	public async Task DeleteMessage(Guid messageId)
	{
		await service.DeleteMessageAsync(messageId);
	}
}
