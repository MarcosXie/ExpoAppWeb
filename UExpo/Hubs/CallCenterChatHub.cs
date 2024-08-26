using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.Entities.CallCenterChat;

namespace UExpo.Api.Hubs;

public class CallCenterChatHub(ICallCenterChatService service) : Hub
{
    private readonly string _adminRoom = "AdminRoom";

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

    public async Task LeaveRoom(CallCenterChatDto callCenterChat)
    {
        string roomId = callCenterChat.Id.ToString();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Groups(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the room.");
    }

    public async Task<List<CallCenterChatResponseDto>> JoinAdminRoom()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, _adminRoom);
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
            await Clients.Groups(_adminRoom).SendAsync("UpdatedChats", await service.GetChatsAsync());
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
