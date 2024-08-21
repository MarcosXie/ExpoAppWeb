using UExpo.Domain.Shared;

namespace UExpo.Domain.Catalogs.ItemImages;

public class CatalogItemImage : BaseModel
{
    public Guid CatalogId { get; set; }
    public Catalog Catalog { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string ItemId { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public int Order { get; set; }
}
