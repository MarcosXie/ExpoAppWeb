using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Shared;
using UExpo.Domain.Users;

namespace UExpo.Domain.Catalogs;

public class Catalog : BaseModel
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Dictionary<string, object>>? JsonTable { get; set; }

    public ICollection<CatalogItemImage> ItemImages { get; set; } = [];
    public ICollection<CatalogPdf> Pdfs { get; set; } = [];
}