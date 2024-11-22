using ExpoShared.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Expo;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpoController(IExpoService service) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<ExpoResponseDto>> GetExpoAsync()
	{
		ExpoResponseDto expo = await service.GetCurrentExpoAsync();

		return Ok(expo);
	}

	[HttpGet("Exhibitor")]
	public async Task<ActionResult<List<ExhibitorResponseDto>>> GetExhibitorsAsync([FromQuery] ExpoSearchDto searchDto)
	{
		var exhibitors = await service.GetExhibitorsAsync(searchDto);

		return Ok(exhibitors);
	}
}
