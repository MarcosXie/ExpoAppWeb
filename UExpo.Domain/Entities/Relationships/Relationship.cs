using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Chats.RelationshipChat;

namespace UExpo.Domain.Entities.Relationships;

public class Relationship : BaseModel
{
	public Guid BuyerUserId { get; set; }
	public User BuyerUser { get; set; } = null!;
	public string BuyerLang { get; set; } = "en";
	public string BuyerMemo { get; set; } = string.Empty;
	public RelationshipStatus BuyerStatus { get; set; } = RelationshipStatus.Normal;

	public Guid SupplierUserId { get; set; }
	public User SupplierUser { get; set; } = null!;
	public string SupplierLang { get; set; } = "en";
	public string SupplierMemo { get; set; } = string.Empty;
	public RelationshipStatus SupplierStatus { get; set; } = RelationshipStatus.Normal;

	public Guid? CalendarId { get; set; }
	public Calendar? Calendar { get; set; } = null!;

	public List<RelationshipMessage> Messages { get; set; } = [];

	public Guid CartId { get; set; }
}
