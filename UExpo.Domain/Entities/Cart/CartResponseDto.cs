using UExpo.Domain.Entities.Users;
namespace UExpo.Domain.Entities.Cart;

public class CartResponseDto
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public UserProfileResponseDto User { get; set; } = null!;
	public string CartNo { get; set; } = null!;
	public bool IsSupplier { get; set; }
	public CartStatus Status { get; set; } = CartStatus.Active;

	public List<CartItemResponseDto> Items { get; set; } = [];
	public DateTime CreatedAt { get; set; }
}
