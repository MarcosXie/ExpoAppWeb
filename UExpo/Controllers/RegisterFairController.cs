using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Exhibitors;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterFairController(ICalendarFairService service) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<ExhibitorFairRegisterResponseDto>>> GetAsync()
    {
        var registeredFairs = await service.GetRegisteredFairsAsync();

        return Ok(registeredFairs);
    }

    [HttpGet("Options")]
    public async Task<ActionResult<List<CalendarFairOptionResponseDto>>> GetOptionsAsync()
    {
        var fairs = await service.GetUpcomingFairsOptionsAsync();

        return Ok(fairs);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> RegisterAsync(ExhibitorFairRegisterDto registerDto)
    {
        var id = await service.RegisterAsync(registerDto.CalendarFairId);

        return Ok(id);
    }

    [HttpPost("Pay")]
    public async Task<ActionResult<bool>> PayAsync(ExhibitorFairRegisterPayDto payDto)
    {
        var isPaid = await service.PayAsync(payDto.FairRegisterIds);

        return Ok(isPaid);
    }
}
