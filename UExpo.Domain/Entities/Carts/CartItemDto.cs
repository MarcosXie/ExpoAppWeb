namespace UExpo.Domain.Entities.Carts;

public class CartItemDto
{
	public string ItemId { get; set; } = null!;
	public Dictionary<string, string> JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public string? ImgUrl { get; set; }
}
