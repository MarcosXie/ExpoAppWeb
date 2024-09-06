namespace UExpo.Domain.Entities.Calendars;

public class CalendarResponseDto
{
	public Guid Id { get; set; }
	public string Place { get; set; } = null!;
	public int Year { get; set; }
	public DateTime BeginDate { get; set; }
	public DateTime EndDate { get; set; }
}
