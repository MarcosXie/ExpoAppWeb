using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Expo;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class LastSearchedTagsRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<LastSearchedTagsDao, LastSearchedTags>(context, mapper), ILastSearchedTagsRepository
{
	public async Task<LastSearchedTags?> GetByUserIdOrDefaultAsync(Guid id)
	{
		var entity = await Database.AsNoTracking().FirstOrDefaultAsync(x => x.UserId!.Equals(id));

		return entity is null ? default : Mapper.Map<LastSearchedTags>(entity);
	}
}
