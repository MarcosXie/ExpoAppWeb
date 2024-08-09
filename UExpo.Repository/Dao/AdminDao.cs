using UExpo.Domain.Admins;

namespace UExpo.Repository.Dao;

public class AdminDao : BaseDao
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;

    public CallCenterChatDao? CallCenterChat { get; set; }
}
