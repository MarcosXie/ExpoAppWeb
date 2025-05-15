using ExpoShared.Domain.Shared;

namespace ExpoApp.Domain.Entities.Wed;

public class Present : BaseModel
{
	public required string Name { get; set; }
	public required double Price { get; set; }
	public required string ImageUri { get; set; }
	public required string PurchaseLink { get; set; }
	public  bool ByPix { get; set; }
	public string? Buyer { get; set; }
}
