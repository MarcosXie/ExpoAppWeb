using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Entities.Expo;

namespace UExpo.Api.Controllers;

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
