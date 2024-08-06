namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatService
{
    Task<(Guid, string)> CreateCallCenterChatAsync(CallCenterChatDto chat);
    Task<CallCenterReceiveMessageDto> AddMessageAsync(CallCenterSendMessageDto message);
}
