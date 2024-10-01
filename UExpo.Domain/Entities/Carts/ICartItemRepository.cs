using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Carts;

public interface ICartItemRepository : IBaseRepository<CartItemDao, CartItem>
{
}

