using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Agendas;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AgendaController(IAgendaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AgendaResponseDto>>> GetAsync()
    {
        var agendas = await service.GetAsync();

        return Ok(agendas);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateAsync(AgendaDto place)
    {
        var id = await service.CreateAsync(place);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(Guid id, AgendaDto agenda)
    {
        await service.UpdateAsync(id, agenda);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        await service.DeleteAsync(id);

        return Ok();
    }
}
