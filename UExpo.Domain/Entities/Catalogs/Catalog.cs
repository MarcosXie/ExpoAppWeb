using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Entities.Users;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Catalogs;

public class Catalog : BaseModel
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Dictionary<string, object>>? JsonTable { get; set; }

    public ICollection<CatalogItemImage> ItemImages { get; set; } = [];
    public ICollection<CatalogPdf> Pdfs { get; set; } = [];
}