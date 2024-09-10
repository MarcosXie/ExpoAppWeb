using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CallCenterMessageDao : BaseMessageDao
{
    public CallCenterChatDao Chat { get; set; } = null!;
}
