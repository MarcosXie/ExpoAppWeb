using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Domain.Entities.Chats.CallCenterChat;

public interface ICallCenterChatService
{
	Task<Guid> CreateCallCenterChatAsync(ChatDto chat);
	Task<(ReceiveMessageDto, bool)> AddMessageAsync(SendMessageDto message);
	Task UpdateChatAsync(ChatDto callCenterChat);
	Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto callCenterChat);
	Task<List<CallCenterChatResponseDto>> GetChatsAsync();
	Task VisualizeMessagesAsync(ChatDto callCenterChat);
	Task<CallCenterChatResponseDto> GetChatByUserIdAsync(string? language);
	Task<int> GetNotReadedMessagesByUserId(Guid userId);
	Task DeleteMessageAsync(DeleteMsgDto message);
}
