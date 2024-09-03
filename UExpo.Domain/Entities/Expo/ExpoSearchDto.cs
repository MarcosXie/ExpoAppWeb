namespace UExpo.Domain.Entities.Expo;

public class ExpoSearchDto
{
	public Guid CalendarId { get; set; }
	public List<Guid> Fairs { get; set; } = [];
	public List<Guid> Segments { get; set; } = [];
	public List<string> Tags { get; set; } = [];
}

