using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendars.Segments;

public class CalendarSegment : BaseModel
{
	public string Name { get; set; } = null!;

	public Guid FairId { get; set; }
	public CalendarFair Fair { get; set; } = null!;
}
