using UExpo.Domain.Authentication;

namespace UExpo.Domain.CallCenterChat;

public interface ICallCenterChatRepository
{
    Task<Guid> CreateAsync(CallCenterChat item, CancellationToken cancellationToken = default);
    Task<Guid> AddMessageAsync(CallCenterMessage message, CancellationToken cancellationToken = default);
    Task<List<CallCenterChat>> GetAsync(CancellationToken cancellationToken = default);
    Task<CallCenterChat> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CallCenterChat?> GetByIdOrDefaultAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(CallCenterChat item, CancellationToken cancellationToken = default);
    Task<List<CallCenterMessage>> GetLastMessagesByChat(Guid id);
    Task<List<CallCenterChat>> GetWithUsersAsync();
    Task<CallCenterChat> GetOrCreateUserChatAsync(AuthenticatedUser authenticatedUser);
    Task<CallCenterChat> GetByUserIdAsync(Guid id);
    Task VisualizeMessagesAsync(CallCenterChatDto callCenterChat);
    Task<int> GetNotReadedMessagesByChatId(Guid roomId);
    Task<int> GetNotReadedMessagesByUserId(Guid userId);
}
