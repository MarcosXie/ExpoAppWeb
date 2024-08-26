using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Fairs.Segments;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SegmentController(ISegmentService service) : ControllerBase
{

    [HttpGet("Fair/{id}")]
    public async Task<ActionResult<List<SegmentResponseDto>>> GetAsync(Guid id)
    {
        var fairs = await service.GetByFairAsync(id);

        return Ok(fairs);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(SegmentDto segment)
    {
        var id = await service.CreateAsync(segment);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await service.DeleteAsync(id);

        return Ok();
    }
}
