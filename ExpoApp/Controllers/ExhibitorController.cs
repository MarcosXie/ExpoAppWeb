using ExpoApp.Domain.Entities.Exhibitors;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExhibitorController(IExhibitorService exhibitorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ExpoFinderResponseDto>> GetExpoFinderAsync(
	    [FromQuery] string? companyName = null,
	    [FromQuery] string? name = null,
	    [FromQuery] string? country = null
	)
    {
        var exhibitors = await exhibitorService.GetExhibitorsAsync(companyName, name, country);
        var options = await exhibitorService.GetFinderOptionsAsync();
        return Ok(new ExpoFinderResponseDto
        {
	        Exhibitors = exhibitors,
	        CompanyNames = options.CompanyNames,
	        Countries = options.Countries,
	        Names = options.Names
        });
    }
}
