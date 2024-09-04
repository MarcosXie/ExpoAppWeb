namespace UExpo.Domain.Entities.Expo;

public interface ILastSearchedTagsService
{
	Task UpdateAsync(Guid userId, LastSearchedTagsDto lastSearchedTags);
	Task<LastSearchedTagsResponseDto?> GetByUserIdAsync(Guid userId);
}
