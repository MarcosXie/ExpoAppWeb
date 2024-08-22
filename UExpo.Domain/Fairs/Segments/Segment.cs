using UExpo.Domain.Shared;

namespace UExpo.Domain.Fairs.Segments;

public class Segment : BaseModel
{
    public string Name { get; set; } = null!;
    public Guid FairId { get; set; }
}
