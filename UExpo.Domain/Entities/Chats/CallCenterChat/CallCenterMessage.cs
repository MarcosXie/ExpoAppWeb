using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Domain.Entities.Chats.CallCenterChat;

public class CallCenterMessage : BaseMessage
{
	public CallCenterChat Chat { get; set; } = null!;
}
