using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Entities.Catalogs.ItemImages;

public interface ICatalogItemImageRepository : IBaseRepository<CatalogItemImageDao, CatalogItemImage>
{
    Task<List<CatalogItemImage>> GetMainImagesByCatalogAsync(Guid id);
    Task<int> GetMaxOrderByProductAsync(string id);
}
