using UExpo.Domain.Entities.Chats.Shared;
using UExpo.Domain.Entities.Relationships;

namespace UExpo.Domain.Entities.Chats.RelationshipChat;

public class RelationshipMessage : BaseMessage 
{
	public Relationship Chat { get; set; } = null!;
}
