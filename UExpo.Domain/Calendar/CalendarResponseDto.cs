namespace UExpo.Domain.Calendar;

public class CalendarReponseDto
{
    public Guid Id { get; set; }
    public string Place { get; set; } = null!;
    public int Year { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
}
