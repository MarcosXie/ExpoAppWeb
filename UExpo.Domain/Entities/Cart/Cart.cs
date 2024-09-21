using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Cart;

public class Cart : BaseModel
{
	public Guid BuyerUserId { get; set; }
	public User BuyerUser { get; set; } = null!;
	public string BuyerLang { get; set; } = "en";
	public Guid SupplierUserId { get; set; }
	public User SupplierUser { get; set; } = null!;
	public string SupplierLang { get; set; } = "en";
	public string CartNo { get; set; } = null!;
	public bool IsFavorite { get; set; } = false;
	public CartStatus Status { get; set; } = CartStatus.Active;

	public List<CartItem> Items { get; set; } = [];
}
