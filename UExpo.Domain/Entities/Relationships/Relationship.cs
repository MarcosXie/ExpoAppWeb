using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;
using UExpo.Domain.Entities.Calendars;

namespace UExpo.Domain.Entities.Relationships;

public class Relationship : BaseModel
{
	public Guid BuyerUserId { get; set; }
	public User BuyerUser { get; set; } = null!;
	public Guid SupplierUserId { get; set; }
	public User SupplierUser { get; set; } = null!;
	public Guid CalendarId { get; set; }
	public Calendar Calendar { get; set; } = null!;
	public Guid ChatId { get; set; }
	//TODO: Adds Chat entity
}
