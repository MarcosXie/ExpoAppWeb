using UExpo.Domain.Entities.Calendars.Segments;

namespace UExpo.Domain.Entities.Calendars.Fairs;

public class CalendarFairOptionResponseDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = null!;

	public List<CalendarSegmentOptionResponseDto> Segments { get; set; } = [];
}
