using System.ComponentModel.DataAnnotations.Schema;

namespace UExpo.Domain.Dao;

public class CallCenterChatDao : BaseDao
{
    public Guid UserId { get; set; }
    public UserDao User { get; set; } = null!;

    public Guid? AdminId { get; set; }
    public AdminDao? Admin { get; set; }
    public string UserLang { get; set; } = "en-US";
    public string AdminLang { get; set; } = "pt-BR";
    [NotMapped]
    public int NotReadedMessages { get; set; }

    public ICollection<CallCenterMessageDao> Messages { get; set; } = [];
}
