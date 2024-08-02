using Microsoft.AspNetCore.SignalR;

namespace UExpo.Api.Hubs;

public class ChatHub : Hub
{
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the room.");
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Groups(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the room.");
    }

    public async Task SendMessageToRoom (string roomId, string message)
    {
        await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
    }
}
