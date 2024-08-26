using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Fairs;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FairController(IFairService service) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<FairResponseDto>>> GetAsync()
    {
        var fairs = await service.GetAsync();

        return Ok(fairs);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(FairDto fair)
    {
        var id = await service.CreateAsync(fair);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await service.DeleteAsync(id);

        return Ok();
    }
}
