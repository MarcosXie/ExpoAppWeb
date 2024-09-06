using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Tutorial;

namespace UExpo.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class RelationshipController(IRelationshipService service) : ControllerBase
{

	[HttpPost]
	public async Task<ActionResult<Guid>> CreateAsync(RelationshipDto relationship)
	{
		var id = await service.CreateAsync(relationship);

		return Ok(id);
	}

	[HttpGet]
	public async Task<ActionResult<List<RelationshipResponseDto>>> GetAsync([FromQuery] Guid? userId = null)
	{
		var relationships = await service.GetRelationshipsByUserIdAsync(userId);

		return Ok(relationships);
	}
}
