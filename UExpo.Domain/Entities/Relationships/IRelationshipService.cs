namespace UExpo.Domain.Entities.Relationships;

public interface IRelationshipService
{
	Task<List<RelationshipResponseDto>> GetRelationshipsByUserIdAsync(Guid? id);
	Task<Guid> CreateAsync(RelationshipDto relationship);
	Task UpdateStatusAsync(Guid id, RelationshipStatusUpdateDto updateDto);
}
