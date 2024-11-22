using ExpoShared.Domain.Entities.Catalogs;
using ExpoShared.Domain.Entities.Tags;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

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


	[HttpPut("{id}/Segment")]
	public async Task<ActionResult<string>> UpdateSegmentsAsync(Guid id, SegmentTagUpdateDto updateDto)
	{
		await service.UpdateSegmentsAsync(id, updateDto.SegmentIds, updateDto.Tags);

		return Ok();
	}
}
