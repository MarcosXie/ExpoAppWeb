using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.RelationshipChat;
using UExpo.Domain.Entities.Relationships;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class RelationshipMessageRepository(UExpoDbContext context, IMapper mapper)
: BaseRepository<RelationshipMessageDao, RelationshipMessage>(context, mapper), IRelationshipMessageRepository
{ 
}
