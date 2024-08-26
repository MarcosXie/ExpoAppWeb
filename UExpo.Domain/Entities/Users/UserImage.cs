using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Users;

public class UserImage : BaseModel
{
    public string Uri { get; set; } = null!;
    public int Order { get; set; }
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}
