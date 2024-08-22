namespace UExpo.Domain.Fairs.Segments;

public class SegmentResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid FairId { get; set; }
}
