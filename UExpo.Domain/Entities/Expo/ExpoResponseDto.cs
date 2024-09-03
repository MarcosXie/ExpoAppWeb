using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Calendar.Segments;
using UExpo.Domain.Entities.Exhibitors;

namespace UExpo.Domain.Entities.Expo;

public class ExpoResponseDto
{
	public Guid CalendarId { get; set; }
	public DateTime BeginDate { get; set; }
	public DateTime EndDate { get; set; }
	public bool IsStarted { get; set; }
	public string Place { get; set; } = null!;
	public List<CalendarFairOptionResponseDto> Fairs { get; set; } = [];
	public List<CalendarSegmentOptionResponseDto> Segments { get; set; } = [];
	public List<string> Tags { get; set; } = [];
	public List<ExhibitorResponseDto> Exhibitors { get; set; } = [];
}
