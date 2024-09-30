namespace UExpo.Domain.Entities.Relationships;

public interface IRelationshipService
{
	Task<List<RelationshipResponseDto>> GetRelationshipsByUserIdAsync(Guid? id);
	Task<Guid> CreateAsync(RelationshipDto relationship);
	Task UpdateStatusAsync(Guid id, RelationshipStatusUpdateDto updateDto);
	Task<string> GetMemoAsync(Guid id);
	Task UpdateMemoAsync(Guid id, RelationshipMemoUpdateDto memo);
	Task<RelationshipResponseDto> GetByCartIdAsync(Guid cartId);
}
