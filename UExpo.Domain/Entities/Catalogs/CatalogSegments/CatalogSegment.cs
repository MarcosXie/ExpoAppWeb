using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Catalogs.CatalogSegments;

public class CatalogSegment : BaseModel
{
	public Guid CatalogId { get; set; }
	public Catalog Catalog { get; set; } = null!;

	public Guid CalendarSegmentId { get; set; }
	public CalendarSegment CalendarSegment { get; set; } = null!;

	public Guid CalendarId { get; set; }
	public Calendar Calendar { get; set; } = null!;
}
