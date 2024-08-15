namespace UExpo.Domain.Catalogs.ItemImages;

public class CatalogItemImageResponseDto
{
    public Guid Id { get; set; }
    public string ItemId { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public int Order { get; set; }
}
