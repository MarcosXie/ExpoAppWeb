using UExpo.Domain.Entities.Admins;

namespace UExpo.Domain.Dao;

public class AdminDao : BaseDao
{
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public AdminType Type { get; set; }
    public bool Active { get; set; } = true;
	public string Lang { get; set; } = "en";

	public List<CallCenterChatDao>? CallCenterChats { get; set; }
}
