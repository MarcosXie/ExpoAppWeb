using ExpoShared.Domain.Entities.Relationships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

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

	[HttpGet("CartId/{cartId}")]
	public async Task<ActionResult<RelationshipResponseDto>> GetByCartIdAsync(Guid cartId)
	{
		var relationships = await service.GetByCartIdAsync(cartId);

		return Ok(relationships);
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult<RelationshipResponseDto>> GetByIdAsync(Guid id)
	{
		var relationship = await service.GetByIdAsync(id);

		return Ok(relationship);
	}
	[HttpPatch("{id}/Status")]
	public async Task<ActionResult<List<RelationshipResponseDto>>> UpdateStatusAsync(
		Guid id, RelationshipStatusUpdateDto updateDto)
	{
		await service.UpdateStatusAsync(id, updateDto);

		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult<List<RelationshipResponseDto>>> DeleteAsync(Guid id)	{
		await service.DeleteAsync(id);

		return Ok();
	}
	
	[HttpGet("{id}/Memo")]
	public async Task<ActionResult<List<RelationshipResponseDto>>> GetMemoAsync(Guid id)
	{
		var memo = await service.GetMemoAsync(id);

		return Ok(memo);
	}

	[HttpPatch("{id}/Memo")]
	public async Task<ActionResult<List<RelationshipResponseDto>>> UpdateMemoAsync(
	Guid id, RelationshipMemoUpdateDto updateDto)
	{
		await service.UpdateMemoAsync(id, updateDto);

		return Ok();
	}

	[HttpGet("Add/{id}")]
	[AllowAnonymous]
	public ActionResult UpdateMemoAsync(Guid id)
	{
		string appUrl = $"expoapp://add-relationship/{id}";
		string playStoreUrl = "https://google.com";
		string appStoreUrl = "https://google.com";
		
		string fallbackUrl = playStoreUrl;
		string userAgent = Request.Headers["User-Agent"].ToString();

		if (userAgent.Contains("iPhone") || userAgent.Contains("iPad"))
		{
			fallbackUrl = appStoreUrl;
		}

		return Content($@"
			<html>
				<head>
		            <meta http-equiv='refresh' content='0; url={appUrl}' />
		            <script>
		                setTimeout(function() {{
		                    window.location.href = '{fallbackUrl}';
		                }}, 2500);
		            </script>
				</head>
		        <body>
		        </body>
			</html>	
		", "text/html");
	}
}
