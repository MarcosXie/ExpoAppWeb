namespace UExpo.Domain.Entities.Chats.Shared;

public class SendMessageDto
{
	public Guid RoomId { get; set; }
	public Guid SenderId { get; set; }
	public Guid? ResponsedMessageId { get; set; } = null;
	public string SenderName { get; set; } = null!;
	public string SendedMessage { get; set; } = null!;
	public byte[]? File { get; set; }
	public string? FileName { get; set; }
}
