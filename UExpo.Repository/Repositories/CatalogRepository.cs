using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogDao, Catalog>(context, mapper), ICatalogRepository
{
    public async Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id)
    {
        var catalog = await Database.FirstOrDefaultAsync(x => x.UserId == id);

        return catalog is null ? null : Mapper.Map<Catalog>(catalog);
    }
}
