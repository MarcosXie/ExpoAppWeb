using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Expo;

public interface ILastSearchedTagsRepository : IBaseRepository<LastSearchedTagsDao, LastSearchedTags>
{
	Task<LastSearchedTags?> GetByUserIdOrDefaultAsync(Guid id);
}
