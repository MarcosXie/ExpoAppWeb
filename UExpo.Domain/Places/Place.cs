using UExpo.Domain.Shared;

namespace UExpo.Domain.Places;

public class Place : BaseModel
{
    public string Name { get; set; } = null!;
    public int Year { get; set; }
    public bool Active { get; set; } = true;
}
