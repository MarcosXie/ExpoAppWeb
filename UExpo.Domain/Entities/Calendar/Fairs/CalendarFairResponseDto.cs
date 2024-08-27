namespace UExpo.Domain.Entities.Calendar.Fairs;

public class CalendarFairResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
