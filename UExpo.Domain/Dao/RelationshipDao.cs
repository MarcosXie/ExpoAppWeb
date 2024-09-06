namespace UExpo.Domain.Dao;

public class RelationshipDao : BaseDao
{
	public Guid BuyerUserId { get; set; }
	public UserDao BuyerUser { get; set; } = null!;
	public Guid SupplierUserId { get; set; }
	public UserDao SupplierUser { get; set; } = null!;
	public Guid CalendarId { get; set; }
	public CalendarDao Calendar { get; set; } = null!;
	public Guid ChatId { get; set; }
	//TODO: Adds Chat entity
}
