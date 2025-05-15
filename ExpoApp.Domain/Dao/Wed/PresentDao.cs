using ExpoShared.Domain.Dao.Shared;

namespace ExpoApp.Domain.Dao.Wed;

public class PresentDao : BaseDao
{
	public required string Name { get; set; }
	public required double Price { get; set; }
	public required string ImageUri { get; set; }
	public required string PurchaseLink { get; set; }
	public  bool ByPix { get; set; }
	public string? Buyer { get; set; }
}
