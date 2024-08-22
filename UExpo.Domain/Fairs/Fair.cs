using UExpo.Domain.Shared;

namespace UExpo.Domain.Fairs;

public class Fair : BaseModel
{
    public string Name { get; set; } = null!;
    public bool Active { get; set; } = false;
}
