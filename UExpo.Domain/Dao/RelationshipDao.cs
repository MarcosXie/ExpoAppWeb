using UExpo.Domain.Dao.Shared;
using UExpo.Domain.Entities.Relationships;

namespace UExpo.Domain.Dao;

public class RelationshipDao : BaseDao
{
	public Guid BuyerUserId { get; set; }
	public UserDao BuyerUser { get; set; } = null!;
	public string BuyerLang { get; set; } = "en";
	public string BuyerMemo { get; set; } = string.Empty;
	public RelationshipStatus BuyerStatus { get; set; } = RelationshipStatus.Normal;

	public Guid SupplierUserId { get; set; }
	public UserDao SupplierUser { get; set; } = null!;
	public string SupplierLang { get; set; } = "en";
	public string SupplierMemo { get; set; } = string.Empty;
	public RelationshipStatus SupplierStatus { get; set; } = RelationshipStatus.Normal;

	public Guid CalendarId { get; set; }
	public CalendarDao Calendar { get; set; } = null!;

	public ICollection<RelationshipMessageDao> Messages { get; set; } = [];
}
