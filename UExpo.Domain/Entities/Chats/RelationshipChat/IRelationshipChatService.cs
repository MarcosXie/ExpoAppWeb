using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Domain.Entities.Chats.RelationshipChat;

public interface IRelationshipChatService
{
	Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto message);
	Task<List<BaseMessage>> GetMessagesByChatAsync(ChatDto joinChatDto);
	Task<List<RelationshipNotReadedMessagesDto>> GetNotReadedMessagesAsync();
	Task UpdateLangAsync(ChatDto callCenterChat);
	Task VisualizeMessagesAsync(ChatDto chat);
}
