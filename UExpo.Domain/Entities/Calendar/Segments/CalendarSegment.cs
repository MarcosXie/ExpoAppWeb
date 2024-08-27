using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendar.Segments;

public class CalendarSegment : BaseModel
{
    public string Name { get; set; } = null!;

    public Guid FairId { get; set; }
    public CalendarFair Fair { get; set; } = null!;
}
