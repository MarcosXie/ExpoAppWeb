using UExpo.Domain.Catalogs.Pdfs;

namespace UExpo.Domain.Catalogs;

public interface ICatalogService
{
    Task<CatalogResponseDto> GetOrCreateAsync(string id);
    Task<Guid> AddPdfAsync(CatalogPdfDto tag);
    Task DeletePdfAsync(Guid id, Guid pdfId);
}
