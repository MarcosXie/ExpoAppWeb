using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Cart;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CartRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<CartDao, Cart>(context, mapper), ICartRepository
{
	public async Task<Cart> GetByIdDetailedAsync(Guid id)
	{
		var entity = await Database
			.Include(x => x.SupplierUser)
			.Include(x => x.BuyerUser)
			.Include(x => x.Items)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id!.Equals(id));

		return entity is null
			? throw new Exception($"{nameof(CartDao)} com id = {id}")
		: Mapper.Map<Cart>(entity);
	}

	public async Task<List<Cart>> GetDetailedAsync(Guid userId)
	{
		var carts = await Database
			.Include (x => x.BuyerUser)
			.Include(x => x.SupplierUser)
			.Include(x => x.Items)
			.AsSplitQuery()
			.Where(x => x.SupplierUserId == userId || x.BuyerUserId == userId && x.Status != CartStatus.Building)
			.ToListAsync();

		return Mapper.Map<List<Cart>>(carts);
	}

	public async Task<List<CartItem>> GetItemsAsync(Guid buyerId, Guid supplierId)
	{
		var items = (await Database
			.Include(x => x.Items)
			.FirstOrDefaultAsync(x =>
				x.Status == CartStatus.Building &&
				x.BuyerUserId == buyerId &&
				x.SupplierUserId == supplierId))
			?.Items;

		return Mapper.Map<List<CartItem>>(items);
	}

	public async Task<string> GetNextCartNoAsync(char separator)
	{
		var currentYear = (DateTime.Now.Year % 100).ToString();

		var cart = await Database
			.Where(x => x.CartNo.EndsWith(currentYear))
			.OrderByDescending(x => x.CartNo)
			.FirstOrDefaultAsync();

		

		return int.TryParse(cart?.CartNo?.Split(separator)[0], out int result) ? (result + 1).ToString() : "1";
	}
}
