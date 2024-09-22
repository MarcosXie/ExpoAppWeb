namespace UExpo.Domain.Entities.Cart;

public interface ICartService
{
	Task CreateAsync(CartDto cart);
	Task RemoveItemAsync(Guid id, Guid itemId);
	Task AddItemAsync(Guid id, CartItemDto item);
	Task<List<CartResponseDto>> GetAsync();
	Task<int> GetItemCountAsync(Guid supplierId);
	Task<List<Cart>> GetByRelationshipBuyerIdsAsync(List<Guid> buyerIds);
}
