namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatService
{
    Task CreateCallCenterChatAsync(CallCenterChatDto chat);
    Task<CallCenterMessageDto> AddMessageAsync(CallCenterMessageDto message);
}
