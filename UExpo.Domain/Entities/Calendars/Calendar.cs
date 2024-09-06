using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Calendars;

public class Calendar : BaseModel
{
	public string Place { get; set; } = null!;
	public int Year { get; set; }
	public DateTime BeginDate { get; set; }
	public DateTime EndDate { get; set; }
	public List<CalendarFair> Fairs { get; set; } = [];
	public bool IsLocked { get; set; } = false;
}
