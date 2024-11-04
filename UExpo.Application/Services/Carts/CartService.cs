using AutoMapper;
using ClosedXML.Excel;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Carts;
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

	public async Task<(byte[], string)> GetExportSheetAsync(Guid id)
	{
		var cart = await _repository.GetByIdDetailedAsync(id);

		var buyerName = string.IsNullOrEmpty(cart.BuyerUser.Enterprise)
			? cart.BuyerUser.Name
			: cart.BuyerUser.Enterprise;

		var user = _authUserHelper.GetUser();


		string fileName;
		
		if (user.Id == cart.SupplierUser.Id)
		{
			fileName = $"CartNo-{cart.CartNo}-{buyerName}";
		}
		else
		{
			fileName = $"CartNo-{cart.CartNo}-{cart.SupplierUser.Enterprise}";
		}

		if (fileName.Length > 31)
		{
			fileName = fileName.Substring(0, 31);
		}

		fileName = fileName.Replace(' ', '_');

		using XLWorkbook workbook = BuildWorkbook(cart, fileName);

		using var stream = new MemoryStream();
		workbook.SaveAs(stream);

		return (stream.ToArray(), fileName);
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

		return MapCarts(carts, userId).OrderBy(x => x.Status).ToList();
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

	public async Task<List<CartItemResponseDto>> GetItemsByCartIdAsync(Guid id)
	{
		return _mapper.Map<List<CartItemResponseDto>>(
				await _cartItemRepository.GetAsync(x => x.CartId == id)
			)
			.OrderBy(x => x.CreatedAt)
			.ToList();
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

		if (item.Annotation is not null)
			dbItem.Annotation = item.Annotation;

		await _cartItemRepository.UpdateAsync(dbItem);
	}

	private XLWorkbook BuildWorkbook(Cart cart, string fileName)
	{
		var workbook = new XLWorkbook();

		var worksheet = workbook.Worksheets.Add(fileName);

		var items = _mapper.Map<List<CartItemResponseDto>>(cart.Items);

		worksheet.Cell(1, 1).Value = "Item Id";
		worksheet.Cell(1, 2).Value = "Quantity";
		worksheet.Cell(1, 3).Value = "Price";
		worksheet.Cell(1, 4).Value = "Total";
		worksheet.Cell(1, 5).Value = "Annotation";

		var row = 2;
		var maxColumnIndex = 5;
		foreach (var item in items.OrderBy(x => x.CreatedAt))
		{
			worksheet.Cell(row, 1).Value = item.ItemId;
			worksheet.Cell(row, 2).Value = item.Quantity;
			worksheet.Cell(row, 3).Value = item.Price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
			worksheet.Cell(row, 4).Value = (item.Quantity * item.Price).ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
			worksheet.Cell(row, 5).Value = item.Annotation?.Replace("\n", Environment.NewLine);
			worksheet.Cell(row, 5).Style.Alignment.WrapText = true;

			using var jsonDoc = JsonDocument.Parse(item.JsonData);
			
			var jsonElement = jsonDoc.RootElement;
			int columnIndex = 6;

			foreach (var prop in jsonElement.EnumerateObject())
			{
				if (row == 2)
				{
					worksheet.Cell(1, columnIndex).Value = prop.Name;
				}

				worksheet.Cell(row, columnIndex).Value = prop.Value.ToString();
				columnIndex++;
			}

			if (columnIndex > maxColumnIndex)
			{
				maxColumnIndex = columnIndex - 1;
			}


			row++;
		}

		worksheet.Columns(1, maxColumnIndex).AdjustToContents();

		return workbook;
	}

	public async Task<Cart> GetByIdAsync(Guid cartId)
	{
		return await _repository.GetByIdAsync(cartId);
	}
}
