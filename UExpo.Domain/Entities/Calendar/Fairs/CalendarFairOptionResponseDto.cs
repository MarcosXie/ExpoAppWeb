using UExpo.Domain.Entities.Calendar.Segments;

namespace UExpo.Domain.Entities.Calendar.Fairs;

public class CalendarFairOptionResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public List<CalendarSegmentOptionResponseDto> Segments { get; set; } = [];
}
