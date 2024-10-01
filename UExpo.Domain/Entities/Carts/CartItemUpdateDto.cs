namespace UExpo.Domain.Entities.Carts;

public class CartItemUpdateDto
{
	public double? Quantity { get; set; }
	public double? Price { get; set; }
	public string? Annotation { get; set; }
}
