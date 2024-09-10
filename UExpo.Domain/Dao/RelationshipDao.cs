using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class RelationshipDao : BaseDao
{
	public Guid BuyerUserId { get; set; }
	public UserDao BuyerUser { get; set; } = null!;
	public Guid SupplierUserId { get; set; }
	public UserDao SupplierUser { get; set; } = null!;
	public string SupplierLang { get; set; } = "en";
	public string BuyerLang { get; set; } = "en";
	public Guid CalendarId { get; set; }
	public CalendarDao Calendar { get; set; } = null!;

	public ICollection<RelationshipMessageDao> Messages { get; set; } = [];
}
