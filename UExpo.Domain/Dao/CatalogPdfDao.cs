namespace UExpo.Domain.Dao;

public class CatalogPdfDao : BaseDao
{
    public Guid CatalogId { get; set; }
    public CatalogDao Catalog { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Uri { get; set; } = null!;
}
