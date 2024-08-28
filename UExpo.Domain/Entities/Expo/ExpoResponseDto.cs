using UExpo.Domain.Entities.Calendar.Fairs;
using UExpo.Domain.Entities.Exhibitors;

namespace UExpo.Domain.Entities.Expo;

public class ExpoResponseDto
{
	public DateTime BeginDate { get; set; }
	public DateTime EndDate { get; set; }
	public bool IsStarted { get; set; }
	public List<CalendarFairOptionResponseDto> Fairs { get; set; } = [];
	public List<string> Tags { get; set; } = [];
	public List<ExhibitorResponseDto> Exhibitors { get; set; } = [];
}
