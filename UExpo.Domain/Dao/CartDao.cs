using UExpo.Domain.Dao.Shared;
using UExpo.Domain.Entities.Carts;

namespace UExpo.Domain.Dao;

public class CartDao : BaseDao
{
	public Guid BuyerUserId { get; set; }
	public UserDao BuyerUser { get; set; } = null!;
	public string BuyerLang { get; set; } = "en";
	public Guid SupplierUserId { get; set; }
	public UserDao SupplierUser { get; set; } = null!;
	public string SupplierLang { get; set; } = "en";
	public string CartNo { get; set; } = null!;
	public CartStatus Status { get; set; } = CartStatus.Building;

	public List<CartItemDao> Items { get; set; } = [];
	public List<CartMessageDao> Messages { get; set; } = [];
}

