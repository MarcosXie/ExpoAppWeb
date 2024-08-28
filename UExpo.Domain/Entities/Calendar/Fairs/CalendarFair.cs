using UExpo.Domain.Entities.Calendar.Segments;
using UExpo.Domain.Entities.Exhibitors;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendar.Fairs;

public class CalendarFair : BaseModel
{
    public string Name { get; set; } = null!;
    public Guid CalendarId { get; set; }
    public Calendar Calendar { get; set; } = null!;
    public List<CalendarSegment> Segments { get; set; } = [];
    public List<ExhibitorFairRegister> FairRegisters { get; set; } = [];
}
