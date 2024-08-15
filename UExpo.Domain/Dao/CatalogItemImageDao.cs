namespace UExpo.Domain.Dao;

public class CatalogItemImageDao : BaseDao
{
    public Guid CatalogId { get; set; }
    public CatalogDao Catalog { get; set; } = null!;

    public string ItemId { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public int Order { get; set; }
}
