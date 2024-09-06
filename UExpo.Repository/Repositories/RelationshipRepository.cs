using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Relationships;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class RelationshipRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<RelationshipDao, Relationship>(context, mapper), IRelationshipRepository
{
	public async Task<List<Relationship>> GetByUserIdAsync(Guid id)
	{
		var users = await Database
			.Include(x => x.BuyerUser)
				.ThenInclude(x => x.Images)
			.Include(x => x.SupplierUser)
				.ThenInclude(x => x.Images)
			.Include(x => x.Calendar)
			.Where(x => x.BuyerUserId == id || x.SupplierUserId == id)
			.ToListAsync();

		return Mapper.Map<List<Relationship>>(users);
	}
}
