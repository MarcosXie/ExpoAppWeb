using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Entities.Relationships;

public class RelationshipResponseDto
{
	public Guid Id { get; set; }
	public RelationshipType Type { get; set; }
	public Guid UserId { get; set; }
	public CalendarResponseDto Calendar { get; set; } = null!;
	public UserProfileResponseDto UserProfile { get; set; } = null!;
	public DateTime CreatedAt { get; set; }
}
