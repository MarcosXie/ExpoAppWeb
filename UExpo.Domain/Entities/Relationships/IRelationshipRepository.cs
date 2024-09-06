using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Relationships;

public interface IRelationshipRepository : IBaseRepository<RelationshipDao, Relationship>
{
	Task<List<Relationship>> GetByUserIdAsync(Guid id);
}
