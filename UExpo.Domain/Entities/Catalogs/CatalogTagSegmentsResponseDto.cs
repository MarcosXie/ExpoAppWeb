using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;

namespace UExpo.Domain.Entities.Catalogs;

public class CatalogTagSegmentsResponseDto
{
	public string Tags { get; set; } = string.Empty;
	public List<CalendarFairOptionResponseDto> Fairs { get; set; } = [];
	public List<CalendarSegmentOptionResponseDto> Segments { get; set; } = [];
}
