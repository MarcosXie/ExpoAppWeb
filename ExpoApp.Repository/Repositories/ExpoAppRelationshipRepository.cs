using AutoMapper;
using ExpoShared.Domain.Entities.Carts;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Repository.Context;
using ExpoShared.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExpoApp.Repository.Repositories;

public class ExpoAppRelationshipRepository(UExpoDbContext context, IMapper mapper) : RelationshipRepository(context, mapper)
{
	public override async Task<List<Relationship>> GetByUserIdAsync(Guid id)
	{
		var users = await Database
			.Include(x => x.BuyerUser)
			.ThenInclude(x => x.Images)
			.Include(x => x.SupplierUser)
			.ThenInclude(x => x.Images)
			.Where(x => x.BuyerUserId == id || x.SupplierUserId == id)
			.ToListAsync();

		return Mapper.Map<List<Relationship>>(users);
	}
	
	public override async Task<List<Relationship>> GetByCartIdAsync(Cart cart)
	{
		var users = await Database
			.Include(x => x.BuyerUser)
			.ThenInclude(x => x.Images)
			.Include(x => x.SupplierUser)
			.ThenInclude(x => x.Images)
			.Where(x => x.BuyerUserId == cart.BuyerUserId || x.SupplierUserId == cart.SupplierUserId)
			.ToListAsync();

		return Mapper.Map<List<Relationship>>(users);
	}
}