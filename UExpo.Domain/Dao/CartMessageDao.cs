using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class CartMessageDao : BaseMessageDao
{
	public CartDao Chat { get; set; } = null!;
}

