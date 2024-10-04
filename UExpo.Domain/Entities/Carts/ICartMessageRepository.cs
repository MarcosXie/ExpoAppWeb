using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Chats.CartChat;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Carts;

public interface ICartMessageRepository :  IBaseRepository<CartMessageDao, CartMessage>
{
}

