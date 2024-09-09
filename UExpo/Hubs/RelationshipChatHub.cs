using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.Entities.CallCenterChat;

namespace UExpo.Api.Hubs;

public class RelationshipChatHub(ICallCenterChatService service) : Hub
{
	private readonly string _relationshipNotificationRoom = "RelationshipNotificationRoom";

	public async Task<JoinChatResponseDto> JoinRoom(CallCenterChatDto callCenterChat)
	{
		Guid roomId = await service.CreateCallCenterChatAsync(callCenterChat);

		await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

		callCenterChat.Id = roomId;

		return new()
		{
			Messages = await service.GetMessagesByChatAsync(callCenterChat),
			RoomId = roomId
		};
	}

	public async Task ChangeUserLang(CallCenterChatDto callCenterChat)
	{
		await service.UpdateChatAsync(callCenterChat);
	}

	public async Task<List<CallCenterChatResponseDto>> JoinRelationshipNotificationRoom()
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, _relationshipNotificationRoom);
		return await service.GetChatsAsync();
	}

	public async Task<int> JoinUserRoom(string userId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, userId);
		return await service.GetNotReadedMessagesByUserId(userId);
	}

	public async Task SendMessageToRoom(CallCenterSendMessageDto message)
	{
		(CallCenterReceiveMessageDto msgDto, bool isSendedByUser) = await service.AddMessageAsync(message);

		await Clients.Group(msgDto.RoomId).SendAsync("ReceiveMessage", msgDto);

		if (isSendedByUser)
			await Clients.Groups(_relationshipNotificationRoom)
				.SendAsync("UpdatedChats", await service.GetChatsAsync());
		else
		{
			(int count, string userId) = await service.GetNotReadedMessagesByRoomId(message.RoomId);
			await Clients.Groups(userId).SendAsync("UpdatedCallCenter", count);
		}
	}

	public async Task VisualizeMessages(CallCenterChatDto callCenterChat)
	{
		await service.VisualizeMessagesAsync(callCenterChat);

		await Clients.Group(callCenterChat.Id.ToString()!).SendAsync("VisualizedMessages", callCenterChat.UserId);
	}
}
