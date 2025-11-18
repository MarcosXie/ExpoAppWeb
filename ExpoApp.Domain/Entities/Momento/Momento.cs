using ExpoShared.Domain.Shared;

namespace ExpoApp.Domain.Entities.Momento;

public class Momento : BaseModel
{
	public required Guid UserId { get; set; }
	public ExpoShared.Domain.Entities.Users.User? User { get; set; }
	
	public required Guid TargetUserId { get; set; }
	public required string Value { get; set; }
	public string? Comment { get; set; }
	public int Order { get; set; } = 1;
	public required MomentoType Type { get; set; }
}