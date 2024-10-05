using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Chats.Shared;

public class BaseMessage : BaseModel
{
	public Guid ChatId { get; set; }
	public Guid SenderId { get; set; }
	public Guid? ResponsedMessageId { get; set; } = null!;
	public BaseMessage? ResponsedMessage { get; set; } = null!;
	public string SenderName { get; set; } = null!;
	public string SenderLang { get; set; } = null!;
	public string SendedMessage { get; set; } = null!;
	public string ReceiverLang { get; set; } = null!;
	public string TranslatedMessage { get; set; } = null!;
	public string RoomId { get; set; } = null!;
	public DateTime SendedTime { get; set; }
	public bool Readed { get; set; }
	public bool Deleted { get; set; } = false;
}
