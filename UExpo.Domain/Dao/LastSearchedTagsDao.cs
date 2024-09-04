namespace UExpo.Domain.Dao;

public class LastSearchedTagsDao : BaseDao
{
	public Guid UserId { get; set; }
	public UserDao User { get; set; } = null!;
	public string FairTags { get; set; } = string.Empty;
	public string SegmentTags { get; set; } = string.Empty;
	public string ProductTags { get; set; } = string.Empty;
}
