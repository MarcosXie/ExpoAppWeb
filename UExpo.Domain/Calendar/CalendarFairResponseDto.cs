namespace UExpo.Domain.Calendar;

public class CalendarFairResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
