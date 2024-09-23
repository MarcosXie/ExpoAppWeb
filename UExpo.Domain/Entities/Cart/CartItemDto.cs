namespace UExpo.Domain.Entities.Cart;

public class CartItemDto
{
	public string ItemId { get; set; } = null!;
	public string JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public string? ImgUrl { get; set; }
}
