using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.CallCenterChat;

namespace UExpo.Api.Hubs;

public class CallCenterChatHub(ICallCenterChatService service) : Hub
{
    public async Task<JoinChatResponseDto> JoinRoom(CallCenterChatDto callCenterChat)
    {
        var roomId = await service.CreateCallCenterChatAsync(callCenterChat);

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
        var roomId = callCenterChat.Id.ToString();

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Groups(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the room.");
    }

    public async Task SendMessageToRoom(CallCenterSendMessageDto message)
    {
        var msgDto = await service.AddMessageAsync(message);

        await Clients.Group(msgDto.RoomId).SendAsync("ReceiveMessage", msgDto);
    }

    public async Task VisualizeMessages(CallCenterChatDto callCenterChat)
    {
        await service.VisualizeMessagesAsync(callCenterChat);

        await Clients.Group(callCenterChat.Id.ToString()!).SendAsync("VisualizedMessages", callCenterChat.UserId);
    }
}
