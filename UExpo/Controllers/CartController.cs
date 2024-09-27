using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Cart;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController(ICartService service) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> CreateAsync(CartDto cart)
	{
		await service.CreateAsync(cart);

		return Ok();
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateItem(Guid id, CartStatusUpdateDto status)
	{
		var cartNo = await service.UpdateStatusAsync(id, status);

		return Ok(cartNo);
	}

	[HttpDelete("{id}/Item/{itemId}")]
	public async Task<ActionResult> RemoveAsync(Guid id, Guid itemId)
	{
		await service.RemoveItemAsync(id, itemId);

		return Ok();
	}

	[HttpPost("{id}/Item")]
	public async Task<ActionResult> AddItemAsync(Guid id, CartItemDto item)
	{
		var count = await service.AddItemAsync(id, item);

		return Ok(count);
	}

	[HttpGet]
	public async Task<ActionResult> GetAsync()
	{
		var carts = await service.GetAsync();

		return Ok(carts);
	}

	[HttpGet("Items/{supplierId}")]
	public async Task<ActionResult> GetItemsAsync(Guid supplierId)
	{
		var items = await service.GetItemsAsync(supplierId);

		return Ok(items);
	}

	[HttpGet("{id}/Items")]
	public async Task<ActionResult> GetItemsByCartIdAsync(Guid id)
	{
		List<CartItemResponseDto> items = await service.GetItemsByCartIdAsync(id);

		return Ok(items);
	}

	[HttpPut("Item/{itemId}")]
	public async Task<ActionResult> UpdateItem(Guid itemId, CartItemUpdateDto item)
	{
		await service.UpdateItemAsync(itemId, item);

		return Ok();
	}
}
