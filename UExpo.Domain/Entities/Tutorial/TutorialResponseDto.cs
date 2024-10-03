using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Entities.Tutorial;

public class TutorialResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public UserType Type { get; set; }
    public string Url { get; set; } = null!;
	public string Page { get; set; } = null!;
	public int Order { get; set; }
}
