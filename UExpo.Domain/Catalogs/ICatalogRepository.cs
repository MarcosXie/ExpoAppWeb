using UExpo.Domain.Dao;
using UExpo.Domain.Shared;

namespace UExpo.Domain.Catalogs;

public interface ICatalogRepository : IBaseRepository<CatalogDao, Catalog>
{
    Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id);
}
