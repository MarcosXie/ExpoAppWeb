using Microsoft.AspNetCore.Http;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Catalogs;

public interface ICatalogService
{
    Task<CatalogResponseDto> GetOrCreateAsync(string id);
    Task<CatalogPdfResponseDto> AddPdfAsync(CatalogPdfDto tag);
    Task DeletePdfAsync(Guid id, Guid pdfId);
    Task<List<Dictionary<string, object>>> AddCatalogDataAsync(Guid id, IFormFile data);
    Task<ValidationErrorResponseDto> ValidadeAddCatalogDataAsync(Guid id, IFormFile data);
    Task<List<CatalogItemImageResponseDto>> AddImagesAsync(Guid id, string productId, List<IFormFile> images);
    Task<List<CatalogItemImageResponseDto>> GetImagesByProductAsync(Guid id, string productId);
    Task DeleteImageAsync(Guid id, string productId, Guid imageId);
}
