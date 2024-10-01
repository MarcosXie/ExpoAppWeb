namespace UExpo.Domain.Entities.Carts;

public class CartDto
{
	public Guid SupplierUserId { get; set; }

	public List<CartItemDto> Items { get; set; } = [];
}
