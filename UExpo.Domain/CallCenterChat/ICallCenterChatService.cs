
namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatService
{
    Task<(Guid, string)> CreateCallCenterChatAsync(CallCenterChatDto chat);
    Task<CallCenterReceiveMessageDto> AddMessageAsync(CallCenterSendMessageDto message);
    Task UpdateChatAsync(CallCenterChatDto callCenterChat);
    Task<List<CallCenterReceiveMessageDto>> GetMessagesByChatAsync(CallCenterChatDto callCenterChat);
    Task<List<CallCenterChatResponseDto>> GetChatsAsync();
}
