namespace UExpo.Domain.Entities.Expo;

public class ExpoSearchDto
{
	public Guid CalendarId { get; set; }
	public List<string> Tags { get; set; } = [];
}

