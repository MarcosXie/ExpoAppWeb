using UExpo.Domain.Admins;

namespace UExpo.Domain.Dao;

public class AdminDao : BaseDao
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;

    public List<CallCenterChatDao>? CallCenterChats { get; set; }
}
