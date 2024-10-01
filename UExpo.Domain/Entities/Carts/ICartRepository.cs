using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Carts;

public interface ICartRepository : IBaseChatRepository, IBaseRepository<CartDao, Cart>
{
	Task<Cart> GetByIdDetailedAsync(Guid id);
	Task<List<Cart>> GetDetailedAsync(Guid userId);
	Task<List<CartItem>> GetItemsAsync(Guid buyerId, Guid supplierId);
	Task<string> GetNextCartNoAsync(char separator);
	Task<List<BaseMessage>> GetNotReadedMessages(Guid currentUserId);
}

