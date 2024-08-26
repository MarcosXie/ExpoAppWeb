using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendar;

public class CalendarFair : BaseModel
{
    public string Name { get; set; } = null!;
    public Guid CalendarId { get; set; }
    public Calendar Calendar { get; set; } = null!;
    public List<CalendarSegment> Segments { get; set; } = [];
}
