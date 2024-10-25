namespace UExpo.Domain.Entities.Tags;

public class SegmentTagUpdateDto
{
	public List<Guid> SegmentIds { get; set; } = [];
	public string Tags { get; set; } = "";
}
