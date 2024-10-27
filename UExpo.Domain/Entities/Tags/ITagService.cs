using UExpo.Domain.Entities.Catalogs;

namespace UExpo.Domain.Entities.Tags;

public interface ITagService
{
	Task AddSegmentsForNewRegisterAsync(Guid exhibitorId, List<Guid> fairIds);
	Task<CatalogTagSegmentsResponseDto> GetTagsAsync(Guid id);
	Task UpdateSegmentsAsync(Guid id, List<Guid> segments, string? tags = null);
}
