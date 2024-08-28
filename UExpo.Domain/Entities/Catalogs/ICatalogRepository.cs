using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Catalogs;

public interface ICatalogRepository : IBaseRepository<CatalogDao, Catalog>
{
    Task<Catalog> GetByIdDetailedAsync(Guid id);
    Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id);
    Task<List<CatalogItemImage>> GetImagesByProductIdAsync(Guid id, string productId);
    Task UpdateTagsAsync(Catalog catalog);
}
