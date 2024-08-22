using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Admins;
using UExpo.Domain.FairDates;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FairDateController(IFairDateService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<FairDateResponseDto>>> GetAsync()
    {
        var fairDates = await service.GetAsync();

        return Ok(fairDates);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(FairDateDto fairDate)
    {
        var id = await service.CreateAsync(fairDate);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, FairDateDto fairDate)
    {
        await service.UpdateAsync(id, fairDate);

        return Ok();
    }

    [HttpPut("{id}/SwitchStatus")]
    public async Task<ActionResult<List<AdminResponseDto>>> SwitchStatusAdmin(Guid id)
    {
        await service.SwitchStatusAsync(id);

        return Ok();
    }
}
