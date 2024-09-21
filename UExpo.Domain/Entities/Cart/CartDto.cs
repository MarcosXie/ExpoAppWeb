namespace UExpo.Domain.Entities.Cart;

public class CartDto
{
	public Guid SupplierUserId { get; set; }

	public List<CartItemDto> Items { get; set; } = [];
}
