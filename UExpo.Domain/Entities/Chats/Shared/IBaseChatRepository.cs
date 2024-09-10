namespace UExpo.Domain.Entities.Chats.Shared;

public interface IBaseChatRepository
{
	Task<Guid> AddMessageAsync(BaseMessage message);
	Task<List<BaseMessage>> GetLastMessagesByChat(Guid id);
	Task VisualizeMessagesAsync(ChatDto callCenterChat);
	Task<int> GetNotReadedMessagesByChatId(Guid roomId, Guid? currentUserId = null);
}
