namespace UExpo.Domain.Dao.Shared;

public abstract class BaseMessageDao : BaseDao
{
	public Guid ChatId { get; set; }
	public Guid SenderId { get; set; }
	public string SenderName { get; set; } = null!;
	public string SenderLang { get; set; } = null!;
	public string SendedMessage { get; set; } = null!;
	public string ReceiverLang { get; set; } = null!;
	public string TranslatedMessage { get; set; } = null!;
	public string? File { get; set; }
	public string? FileName { get; set; }
	public bool Deleted { get; set; } = false;
	public bool Readed { get; set; }

	public Guid? ResponsedMessageId { get; set; } = null;
}
