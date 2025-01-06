using ExpoApp.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Exhibitors;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExhibitorController(IExhibitorService exhibitorService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ExhibitorResponseDto>> GetExhibitorsAsync([FromQuery] string? companyName = null)
    {
        var exhibitors = await exhibitorService.GetExhibitorsAsync(companyName);

        return Ok(exhibitors);
    }
    
    [HttpGet("options")]
    public async Task<ActionResult<string>> GetOptionsAsync()
    {
	    var companies = await exhibitorService.GetExhibitorsCompaniesAsync();

	    return Ok(companies);
    }
}
