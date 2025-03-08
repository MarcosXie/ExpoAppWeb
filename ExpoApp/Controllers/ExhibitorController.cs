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
        var exhibitors = await exhibitorService.GetUsersAsync(companyName, name, country);
        var options = await exhibitorService.GetFinderOptionsAsync(companyName, name, country);
        return Ok(new ExpoFinderResponseDto
        {
	        Exhibitors = exhibitors,
	        CompanyNames = options.CompanyNames.OrderBy(x => x).ToList(),
	        Countries = options.Countries.Distinct().OrderBy(x => x).ToList(),
	        Emails = options.Emails.OrderBy(x => x).ToList()
        });
    }
}
