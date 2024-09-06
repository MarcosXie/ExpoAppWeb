using Microsoft.AspNetCore.Mvc;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;

namespace UExpo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalendarController(ICalendarService service, ICalendarFairService calendarFairService) : ControllerBase
{
    [HttpGet("Year")]
    public async Task<ActionResult<List<int>>> GetYearsAsync()
    {
        var years = await service.GetYearsAsync();

        return Ok(years);
    }

    [HttpGet("IsLocked/{year}")]
    public async Task<ActionResult<bool>> GetIsLockedAsync(int year)
    {
        var isLocked = await service.GetIsLockedAsync(year);

        return Ok(isLocked);
    }

    [HttpPost("Execute/{year}")]
    public async Task<ActionResult> ExecuteAsync(int year)
    {
        await service.ExecuteAsync(year);

        return Ok();
    }

    [HttpPost("Lock/{year}")]
    public async Task<ActionResult> LockAsync(int year)
    {
        await service.LockAsync(year);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<CalendarResponseDto>>> GetAsync([FromQuery] int? year)
    {
        var calendars = await service.GetCalendarsAsync(year);

        return Ok(calendars);
    }

    [HttpGet("Fair")]
    public async Task<ActionResult<List<CalendarFairResponseDto>>> GetFairsAsync([FromQuery] int? year)
    {
        var fairs = await calendarFairService.GetFairsAsync(year);

        return Ok(fairs);
    }

    [HttpGet("Fair/Next")]
    public async Task<ActionResult<List<CalendarFairResponseDto>>> GetNextFairsAsync([FromQuery] int? year)
    {
        var fairs = await calendarFairService.GetUpcomingFairsAsync(year);

        return Ok(fairs);
    }
}
