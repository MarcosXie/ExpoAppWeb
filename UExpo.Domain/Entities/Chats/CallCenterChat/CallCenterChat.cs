using UExpo.Domain.Entities.Admins;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Chats.CallCenterChat;

public class CallCenterChat : BaseModel
{
	public Guid UserId { get; set; }
	public User User { get; set; } = null!;
	public Guid? AdminId { get; set; }
	public Admin? Admin { get; set; }
	public string UserLang { get; set; } = "en-US";
	public string AdminLang { get; set; } = "pt-BR";
	public int NotReadedMessages { get; set; }

	public List<CallCenterMessage> Messages { get; set; } = [];
}
