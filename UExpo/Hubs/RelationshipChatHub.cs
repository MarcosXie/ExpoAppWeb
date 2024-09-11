using Microsoft.AspNetCore.SignalR;
using UExpo.Api.Hubs.Interfaces;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs;

public class RelationshipChatHub(IRelationshipChatService service) : Hub, IChatHub
{
	private readonly string _relationshipNotificationRoom = "RelationshipNotificationRoom";

	public async Task<JoinChatResponseDto> JoinChatRoom(ChatDto joinChatDto)
	{
		var roomId = joinChatDto.Id;

		await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

		return new()
		{
			RoomId = roomId,
			Messages = await service.GetMessagesByChatAsync(joinChatDto),
		};
	}

	public async Task ChangeUserLang(ChatDto callCenterChat)
	{
		await service.UpdateLangAsync(callCenterChat);
	}

	public async Task SendMessage(SendMessageDto message)
	{
		ReceiveMessageDto msgDto = await service.AddMessageAsync(message);

		await Clients.Group(msgDto.RoomId).SendAsync("ReceiveMessage", msgDto);

		await Clients.Groups(msgDto.ReceiverId.ToString()).SendAsync("Notification",
			new UserRoomNotificationsDto
			{
				RelationshipNotifications = await service.GetNotReadedMessagesAsync(message.RoomId),
			});
	}

	public async Task VisualizeMessages(ChatDto chat)
	{
		await service.VisualizeMessagesAsync(chat);

		await Clients.Group(chat.Id.ToString()!).SendAsync("VisualizedMessages", chat.UserId);
	}
}
