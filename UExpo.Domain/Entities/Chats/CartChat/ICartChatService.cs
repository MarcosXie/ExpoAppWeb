using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Domain.Entities.Chats.CartChat;

public interface ICartChatService
{
	Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto message);
	Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto joinChatDto);
	Task<List<NotReadedMessagesDto>> GetNotReadedMessagesAsync(Guid userId);
	Task UpdateLangAsync(ChatDto callCenterChat);
	Task VisualizeMessagesAsync(ChatDto chat);
}
