using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CallCenterChatDao : BaseDao
{
    public Guid UserId { get; set; }
    public UserDao User { get; set; } = null!;

    public Guid? AdminId { get; set; }
    public AdminDao? Admin { get; set; }
    public string UserLang { get; set; } = "en";
    public string AdminLang { get; set; } = "pt";

    public ICollection<CallCenterMessageDao> Messages { get; set; } = [];
}
