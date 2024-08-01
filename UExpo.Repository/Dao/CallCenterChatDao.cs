namespace UExpo.Repository.Dao;

public class CallCenterChatDao : BaseDao
{
    public Guid UserDaoId { get; set; }
    public UserDao UserDao { get; set; } = null!;

    public Guid AttendentId { get; set; }
    public string UserLang { get; set; } = "en-US";
    public string AttendentLang { get; set; } = "pt-BR";

    public ICollection<CallCenterMessageDao> Messages { get; set; } = [];
}
