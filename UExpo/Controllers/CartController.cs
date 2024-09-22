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

	[HttpDelete("{id}/Item/{itemId}")]
	public async Task<ActionResult> RemoveAsync(Guid id, Guid itemId)
	{
		await service.RemoveItemAsync(id, itemId);

		return Ok();
	}

	[HttpPost("{id}/Item")]
	public async Task<ActionResult> AddItemAsync(Guid id, CartItemDto item)
	{
		await service.AddItemAsync(id, item);

		return Ok();
	}

	[HttpGet]
	public async Task<ActionResult> GetAsync()
	{
		var carts = await service.GetAsync();

		return Ok(carts);
	}

	[HttpGet("{supplierId}/Items/Count")]
	public async Task<ActionResult> GetItemCountAsync(Guid supplierId)
	{
		int itemCount = await service.GetItemCountAsync(supplierId);

		return Ok(itemCount);
	}
}
