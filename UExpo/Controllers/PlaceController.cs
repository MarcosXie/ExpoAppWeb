using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Places;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlaceController(IPlaceService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PlaceResponseDto>>> GetAsync()
    {
        var places = await service.GetAsync();

        return Ok(places);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(PlaceDto place)
    {
        var id = await service.CreateAsync(place);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, PlaceDto place)
    {
        await service.UpdateAsync(id, place);

        return Ok();
    }
}
