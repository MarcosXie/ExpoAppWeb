using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Catalogs.Pdfs;

public class CatalogPdf : BaseModel
{
    public Guid CatalogId { get; set; }
    public Catalog Catalog { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Uri { get; set; } = null!;
}
