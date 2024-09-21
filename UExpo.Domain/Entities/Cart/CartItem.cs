namespace UExpo.Domain.Entities.Cart;

public class CartItem
{
	public string ItemId { get; set; } = null!;
	public Guid CartId { get; set; }
	public Cart Cart { get; set; } = null!;
	public string JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public double Price { get; set; }
}
