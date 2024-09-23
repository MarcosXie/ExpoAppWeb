using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CartItemDao : BaseDao
{
	public string ItemId { get; set; } = null!;
	public Guid CartId { get; set; }
	public CartDao Cart { get; set; } = null!;
	public string JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public double Price { get; set; }
	public string? ImgUrl { get; set; }
}
