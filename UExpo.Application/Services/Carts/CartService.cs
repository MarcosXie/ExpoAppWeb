using AutoMapper;
using Newtonsoft.Json;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Cart;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Carts;

public class CartService : ICartService
{
	private AuthUserHelper _authUserHelper;
	private ICartRepository _repository;
	private ICartItemRepository _cartItemRepository;
	private IMapper _mapper;
	private char _cartNoSeparator = '-';

	public CartService(
		ICartRepository repository, 
		AuthUserHelper authUserHelper, 
		IMapper mapper,
		ICartItemRepository cartItemRepository
	)
	{
		_authUserHelper = authUserHelper;
		_repository = repository;
		_cartItemRepository = cartItemRepository;
		_mapper = mapper;
	}

	public async Task CreateAsync(CartDto cart)
	{
		var nextCartNo = await _repository.GetNextCartNoAsync(_cartNoSeparator);
		var dbCart = _mapper.Map<Cart>(cart);

		dbCart.BuyerUserId = _authUserHelper.GetUser().Id;
		dbCart.CartNo = $"{nextCartNo}{_cartNoSeparator}{DateTime.Now.Year % 100}";

		await _repository.CreateAsync(dbCart);
	}

	public async Task<List<CartItemResponseDto>> AddItemAsync(Guid id, CartItemDto item)
	{
		var cart = await _repository.GetByIdDetailedAsync(id);

		if (cart.Items.Any(x => x.ItemId == item.ItemId))
			return _mapper.Map<List<CartItemResponseDto>>(cart.Items);

		var cartItem = _mapper.Map<CartItem>(item);
		cartItem.CartId = cart.Id;
		cartItem.JsonData = JsonConvert.SerializeObject(item.JsonData);

		cartItem.Id = await _cartItemRepository.CreateAsync(cartItem);
		cart.Items.Add(cartItem);

		return _mapper.Map<List<CartItemResponseDto>>(cart.Items);
	}

	public async Task RemoveItemAsync(Guid id, Guid itemId)
	{
		var _ = await _repository.GetByIdDetailedAsync(id);

		await _cartItemRepository.DeleteAsync(itemId);
	}

	public async Task<List<CartResponseDto>> GetAsync()
	{
		var userId = _authUserHelper.GetUser().Id;

		List<Cart> carts = await _repository.GetDetailedAsync(userId);

		return MapCarts(carts, userId).ToList();
	}

	public async Task<List<CartItemResponseDto>> GetItemsAsync(Guid supplierId)
	{
		var buyerId = _authUserHelper.GetUser().Id;

		return _mapper.Map<List<CartItemResponseDto>>(await _repository.GetItemsAsync(buyerId, supplierId));
	}

	public async Task<List<Cart>> GetByRelationshipBuyerIdsAsync(List<Guid> buyerIds)
	{
		return await _repository.GetAsync(x => buyerIds.Contains(x.BuyerUserId) );
	}

	public async Task<string> UpdateStatusAsync(Guid id, CartStatusUpdateDto status)
	{
		var cart = await _repository.GetByIdAsync(id);

		cart.Status = status.Status;

		if (status.Status == CartStatus.Active)
			cart.CreatedAt = DateTime.Now;

		await _repository.UpdateAsync(cart);

		if (status.Status == CartStatus.Active)
		{
			await CreateAsync(new CartDto()
			{
				SupplierUserId = cart.SupplierUserId,
			});
		}

		return cart.CartNo;
	}

	private IEnumerable<CartResponseDto> MapCarts(List<Cart> carts, Guid userId)
	{
		foreach (var cart in carts)
		{
			yield return new()
			{
				Id = cart.Id,
				CartNo = cart.CartNo,
				CreatedAt = cart.CreatedAt,
				Status = cart.Status,
				IsSupplier = userId == cart.SupplierUserId,
				User = _mapper.Map<UserProfileResponseDto>(userId == cart.BuyerUserId ? cart.SupplierUser : cart.BuyerUser),
				UserId = userId == cart.BuyerUserId ? cart.SupplierUserId : cart.BuyerUserId,
				Items = _mapper.Map<List<CartItemResponseDto>>(cart.Items)
			};
		}
	}

	public async Task UpdateItemAsync(Guid itemId, CartItemUpdateDto item)
	{
		var dbItem = await _cartItemRepository.GetByIdAsync(itemId);

		if (item.Quantity is not null)
			dbItem.Quantity = (double)item.Quantity;

		if (item.Price is not null)
			dbItem.Price = (double)item.Price;

		await _cartItemRepository.UpdateAsync(dbItem);
	}
}
