using AutoMapper;
using ExpoShared.Domain.Entities.Users;
using ExpoShared.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace ExpoApp.Repository.Repositories;

public class ExpoAppUserRepository(UExpoDbContext context, IMapper mapper) : ExpoShared.Repository.Repositories.UserRepository(context, mapper)
{
	public override async Task<User> GetByIdDetailedAsync(Guid id)
	{
		var entity = await Database
			.Include(x => x.Images)
			.Include(x => x.Catalog)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id!.Equals(id));

		return entity is null
			? throw new Exception($"{nameof(User)} com id = {id}")
			: Mapper.Map<User>(entity);
	}
}