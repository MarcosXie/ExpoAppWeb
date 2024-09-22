using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Cart;

public interface ICartItemRepository : IBaseRepository<CartItemDao, CartItem>
{
}

