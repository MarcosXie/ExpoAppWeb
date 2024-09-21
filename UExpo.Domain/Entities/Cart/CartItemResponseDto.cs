namespace UExpo.Domain.Entities.Cart;

public class CartItemResponseDto
{
	public Guid Id { get; set; }
	public string ItemId { get; set; } = null!;
	public string JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public double Price { get; set; }
}
