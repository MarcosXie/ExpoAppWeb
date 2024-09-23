namespace UExpo.Domain.Entities.Cart;

public interface ICartService
{
	Task CreateAsync(CartDto cart);
	Task RemoveItemAsync(Guid id, Guid itemId);
	Task<List<CartItemResponseDto>> AddItemAsync(Guid id, CartItemDto item);
	Task<List<CartResponseDto>> GetAsync();
	Task<List<CartItemResponseDto>> GetItemsAsync(Guid supplierId);
	Task<List<Cart>> GetByRelationshipBuyerIdsAsync(List<Guid> buyerIds);
	Task UpdateItemAsync(Guid itemId, CartItemUpdateDto item);
}
