using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Catalogs;

public interface ICatalogRepository : IBaseRepository<CatalogDao, Catalog>
{
    Task<Catalog> GetByIdDetailedAsync(Guid id);
    Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id);
    Task<List<CatalogItemImage>> GetImagesByProductIdAsync(Guid id, string productId);
}
