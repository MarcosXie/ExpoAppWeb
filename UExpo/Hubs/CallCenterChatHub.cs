using Microsoft.AspNetCore.SignalR;
using UExpo.Domain.CallCenterChat;

namespace UExpo.Api.Hubs;

public class CallCenterChatHub(ICallCenterChatService service) : Hub
{
    public async Task<List<CallCenterReceiveMessageDto>> JoinRoom(CallCenterChatDto callCenterChat)
    {
        var (roomId, userName) = await service.CreateCallCenterChatAsync(callCenterChat);

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        CallCenterReceiveMessageDto msg = new()
        {
            SenderId = callCenterChat.UserId,
            TranslatedMessage = $"{userName} has joined the room.",
            SenderName = "System",
            Readed = false
        };

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", msg);

        return await service.GetMessagesByChatAsync(callCenterChat);
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
}
