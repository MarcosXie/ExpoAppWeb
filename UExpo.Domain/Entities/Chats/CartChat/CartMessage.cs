using UExpo.Domain.Entities.Carts;
using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Domain.Entities.Chats.CartChat;

public class CartMessage : BaseMessage
{
	public Cart Chat { get; set; } = null!;
}
