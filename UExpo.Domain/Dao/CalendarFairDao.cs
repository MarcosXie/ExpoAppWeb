namespace UExpo.Domain.Dao;

public class CalendarFairDao : BaseDao
{
    public string Name { get; set; } = null!;
    public Guid CalendarId { get; set; }
    public CalendarDao Calendar { get; set; } = null!;
    public List<CalendarSegmentDao> Segments { get; set; } = [];
}
