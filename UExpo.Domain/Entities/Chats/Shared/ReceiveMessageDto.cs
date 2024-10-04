namespace UExpo.Domain.Entities.Chats.Shared;

public class ReceiveMessageDto
{
	public Guid Id { get; set; }
	public string RoomId { get; set; } = null!;
	public Guid SenderId { get; set; }
	public string SenderName { get; set; } = null!;
	public string SendedMessage { get; set; } = null!;
	public string? TranslatedMessage { get; set; } = null!;
	public DateTime SendedTime { get; set; }
	public bool Readed { get; set; }
	public Guid ReceiverId { get; set; }
}
