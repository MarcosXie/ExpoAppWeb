using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.CallCenterChat;

namespace UExpo.Api.Hubs;

public class CallCenterChatHub(ICallCenterChatService service) : Hub
{
    public async Task JoinRoom(CallCenterChatDto callCenterChat)
    {
        var roomId = callCenterChat.UserId.ToString();

        await service.CreateCallCenterChatAsync(callCenterChat);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the room.");
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Groups(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the room.");
    }

    public async Task SendMessageToRoom(CallCenterMessageDto message)
    {
        var roomId = message.UserId.ToString();

        await service.AddMessageAsync(message);

        await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
    }
}
