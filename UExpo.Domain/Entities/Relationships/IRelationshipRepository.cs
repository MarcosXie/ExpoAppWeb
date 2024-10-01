using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Relationships;

public interface IRelationshipRepository : IBaseChatRepository, IBaseRepository<RelationshipDao, Relationship>
{
	Task<List<Relationship>> GetByCartIdAsync(Cart cart);
	Task<List<Relationship>> GetByUserIdAsync(Guid id);
	Task<List<BaseMessage>> GetNotReadedMessages(Guid currentUserId);
}
