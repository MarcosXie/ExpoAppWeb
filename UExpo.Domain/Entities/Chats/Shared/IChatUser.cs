namespace UExpo.Domain.Entities.Chats.Shared;

public interface IChatUser
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Lang { get; set; }
}
