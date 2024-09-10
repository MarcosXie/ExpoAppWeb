namespace UExpo.Domain.Entities.Chats.Shared;

public class ChatDto
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string Lang { get; set; } = "en";
}
