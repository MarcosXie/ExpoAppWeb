using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Entities.Tutorial;

public class TutorialDto
{
    public string Title { get; set; } = null!;
    public TypeEnum Type { get; set; }
    public string Url { get; set; } = null!;
    public int Order { get; set; }
}
