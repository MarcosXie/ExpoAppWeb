using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Expo;

namespace UExpo.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class LastSearchedTagsController(ILastSearchedTagsService service) : ControllerBase
{

	[HttpPut("{id}")]
	public async Task<ActionResult> UpdateAsync(Guid id, LastSearchedTagsDto lsTags)
	{
		await service.UpdateAsync(id, lsTags);

		return Ok();
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<List<LastSearchedTagsResponseDto>>> GetAsync(Guid id)
	{
		var lsTags = await service.GetByUserIdAsync(id);

		return Ok(lsTags);
	}
}
