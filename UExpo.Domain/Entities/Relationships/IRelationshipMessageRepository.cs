using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Relationships;

public interface IRelationshipMessageRepository : IBaseRepository<RelationshipMessageDao, RelationshipMessage>
{
}
