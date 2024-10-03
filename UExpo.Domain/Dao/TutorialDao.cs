using UExpo.Domain.Dao.Shared;
using UExpo.Domain.Entities.Users;

namespace UExpo.Domain.Dao;

public class TutorialDao : BaseDao
{
    public string Title { get; set; } = null!;
    public UserType Type { get; set; }
    public string Url { get; set; } = null!;
    public string Page { get; set; } = null!;
	public int Order { get; set; }
}
