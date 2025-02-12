using ExpoShared.Domain.Entities.Users;
using ExpoShared.Domain.Shared;

namespace ExpoApp.Domain.Entities.Momento;

public class Momento : BaseModel
{
	public required Guid UserId { get; set; }
	public User? User { get; set; }
	
	public required Guid TargetUserId { get; set; }
	public User? TargetUser { get; set; }
	public required string Value { get; set; }
	public string? Comment { get; set; }
	public required MomentoType Type { get; set; }
}