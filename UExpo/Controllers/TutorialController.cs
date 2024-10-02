using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Tutorial;
using UExpo.Domain.Entities.Users;

namespace UExpo.Api.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class TutorialController(ITutorialService service) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(TutorialDto tutorial)
    {
        var id = await service.CreateAsync(tutorial);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, TutorialDto tutorial)
    {
        await service.UpdateAsync(id, tutorial);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await service.DeleteAsync(id);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<TutorialResponseDto>>> GetAsync([FromQuery] UserType? type)
    {
        var tutorials = await service.GetAsync(type);

        return Ok(tutorials);
    }
}
