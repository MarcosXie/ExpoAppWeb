using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;

namespace UExpo.Domain.Catalogs;

public class CatalogResponseDto
{
    public Guid Id { get; set; }
    public List<Dictionary<string, object>>? JsonTable { get; set; }

    public List<CatalogItemImageResponseDto> ItemImages { get; set; } = [];
    public List<CatalogPdfResponseDto> Pdfs { get; set; } = [];
}
