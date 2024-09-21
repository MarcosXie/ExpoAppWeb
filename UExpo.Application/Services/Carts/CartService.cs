using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Cart;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Carts;

public class CartService : ICartService
{
	private AuthUserHelper _authUserHelper;
	private ICartRepository _repository;
	private IMapper _mapper;
	private char _cartNoSeparator = '-';

	public CartService(ICartRepository repository, AuthUserHelper authUserHelper, IMapper mapper)
	{
		_authUserHelper = authUserHelper;
		_repository = repository;
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

	public async Task AddItemAsync(Guid id, CartItemDto item)
	{
		var cart = await _repository.GetByIdDetailedAsync(id);

		var cartItem = _mapper.Map<CartItem>(item);
		cartItem.CartId = cart.Id;

		cart.Items.Add(cartItem);

		await _repository.UpdateAsync(cart);
	}

	public async Task RemoveItemAsync(Guid id, Guid itemId)
	{
		var cart = await _repository.GetByIdDetailedAsync(id);

		cart.Items = cart.Items.Where(x => x.Id == itemId).ToList();

		await _repository.UpdateAsync(cart);
	}

	public async Task<List<CartResponseDto>> GetAsync()
	{
		var userId = _authUserHelper.GetUser().Id;

		List<Cart> carts = await _repository.GetDetailedAsync(userId);

		return MapCarts(carts, userId).ToList();
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
				IsFavorite = cart.IsFavorite,
				Status = cart.Status,
				User = _mapper.Map<UserProfileResponseDto>(userId == cart.BuyerUserId ? cart.SupplierUser : cart.BuyerUser),
				UserId = userId == cart.BuyerUserId ? cart.SupplierUserId : cart.BuyerUserId,
				Items = _mapper.Map<List<CartItemResponseDto>>(cart.Items)
			};
		}
	}
}
