using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Tags;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController(ITagService service) : ControllerBase
{
	[HttpGet("{id}")]
	public async Task<ActionResult<CatalogTagSegmentsResponseDto>> GetTagsAsync(Guid id)
	{
		var response = await service.GetTagsAsync(id);

		return Ok(response);
	}

	[HttpPut("{id}")]
	public async Task<ActionResult<string>> UpdateTagsAsync(Guid id, CatalogTagDto tags)
	{
		await service.UpdateTagsAsync(id, tags);

		return Ok();
	}

	[HttpPut("{id}/Segment")]
	public async Task<ActionResult<string>> UpdateSegmentsAsync(Guid id, List<Guid> segmentIds)
	{
		await service.UpdateSegmentsAsync(id, segmentIds);

		return Ok();
	}
}
