using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CatalogSegmentDao : BaseDao
{
	public Guid CatalogId { get; set; }
	public CatalogDao Catalog { get; set; } = null!;

	public Guid CalendarSegmentId { get; set; }
	public CalendarSegmentDao CalendarSegment { get; set; } = null!;

	public Guid CalendarId { get; set; }
	public CalendarDao Calendar { get; set; } = null!;
}

