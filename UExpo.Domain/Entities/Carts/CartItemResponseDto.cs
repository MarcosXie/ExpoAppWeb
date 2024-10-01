namespace UExpo.Domain.Entities.Carts;

public class CartItemResponseDto
{
	public Guid Id { get; set; }
	public string ItemId { get; set; } = null!;
	public string JsonData { get; set; } = null!;
	public double Quantity { get; set; }
	public double Price { get; set; }
	public string? ImgUrl { get; set; }
	public string? Annotation { get; set; }
}
