namespace UExpo.Domain.Entities.Expo;

public class LastSearchedTagsResponseDto
{
	public Guid UserId { get; set; }
	public string FairTags { get; set; } = string.Empty;
	public string SegmentTags { get; set; } = string.Empty;
	public string ProductTags { get; set; } = string.Empty;
}
