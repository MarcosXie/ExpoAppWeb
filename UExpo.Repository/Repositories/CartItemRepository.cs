using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Cart;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CartItemRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<CartItemDao, CartItem>(context, mapper), ICartItemRepository
{

}