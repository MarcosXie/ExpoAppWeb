using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Cart;

public interface ICartRepository : IBaseRepository<CartDao, Cart>
{
	Task<Cart> GetByIdDetailedAsync(Guid id);
	Task<List<Cart>> GetDetailedAsync(Guid userId);
	Task<string> GetNextCartNoAsync(char separator);
}

