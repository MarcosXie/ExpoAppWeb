namespace UExpo.Domain.Entities.Relationships;

public class RelationshipDto
{
	public Guid BuyerUserId { get; set; }
	public Guid SupplierUserId { get; set; }
	public Guid CalendarId { get; set; }
}
