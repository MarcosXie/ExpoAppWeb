

namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatService
{
    Task<Guid> CreateCallCenterChatAsync(CallCenterChatDto chat);
    Task<(CallCenterReceiveMessageDto, bool)> AddMessageAsync(CallCenterSendMessageDto message);
    Task UpdateChatAsync(CallCenterChatDto callCenterChat);
    Task<List<CallCenterReceiveMessageDto>> GetMessagesByChatAsync(CallCenterChatDto callCenterChat);
    Task<List<CallCenterChatResponseDto>> GetChatsAsync();
    Task VisualizeMessagesAsync(CallCenterChatDto callCenterChat);
    Task<CallCenterChatResponseDto> GetChatByUserIdAsync();
    Task<(int, string)> GetNotReadedMessagesByRoomId(Guid roomId);
    Task<int> GetNotReadedMessagesByUserId(string userId);
}
