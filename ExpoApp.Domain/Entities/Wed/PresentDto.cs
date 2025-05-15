namespace ExpoApp.Domain.Entities.Wed;

public class PresentDto
{
	public required string Name { get; set; }
	public required double Price { get; set; }
	public required string ImageUri { get; set; }
	public required string PurchaseLink { get; set; }
}
