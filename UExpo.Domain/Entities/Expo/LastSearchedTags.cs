using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Expo;

public class LastSearchedTags : BaseModel
{
	public Guid UserId { get; set; }
	public User User { get; set; } = null!;
	public string FairTags { get; set; } = string.Empty;
	public string SegmentTags { get; set; } = string.Empty;
	public string ProductTags { get; set; } = string.Empty;
}
