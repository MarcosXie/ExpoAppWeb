using UExpo.Domain.Authentication;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Chats.CallCenterChat;

public interface ICallCenterChatRepository : IBaseChatRepository, IBaseRepository<CallCenterChatDao, CallCenterChat>
{
	Task<List<CallCenterChat>> GetWithUsersAsync();
	Task<CallCenterChat> GetOrCreateUserChatAsync(AuthenticatedUser authenticatedUser);
	Task<CallCenterChat?> GetByUserIdAsync(Guid id);
	Task<int> GetNotReadedMessagesByUserId(Guid userId);
}
