using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Tutorial;

public class Tutorial : BaseModel
{
    public string Title { get; set; } = null!;
    public TypeEnum Type { get; set; }
    public string Url { get; set; } = null!;
    public int Order { get; set; }
}
