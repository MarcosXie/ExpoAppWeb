using ExpoApp.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Exhibitors;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExhibitorController(IExhibitorService exhibitorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ExpoFinderResponseDto>>> GetExpoFinderAsync([FromQuery] string? companyName = null)
    {
        var exhibitors = await exhibitorService.GetExhibitorsAsync(companyName);
        var companies = await exhibitorService.GetExhibitorsCompaniesAsync();
        return Ok(new ExpoFinderResponseDto
        {
	        Exhibitors = exhibitors,
	        CompanyNames = companies
        });
    }
}
