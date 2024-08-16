using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogDao, Catalog>(context, mapper), ICatalogRepository
{
    public async Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id)
    {
        CatalogDao? catalog = await Database
            .Include(x => x.Pdfs)
            .Include(x => x.ItemImages)
            .FirstOrDefaultAsync(x => x.UserId == id);

        return catalog is null ? null : Mapper.Map<Catalog>(catalog);
    }

    public override async Task<Catalog> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        CatalogDao? entity = await Database
            .AsNoTracking()
            .Select(x => new CatalogDao
            {
                Id = x.Id,
                ItemImages = x.ItemImages,
                UserId = x.UserId,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken: cancellationToken);

        return entity is null
            ? throw new Exception($"{nameof(CatalogDao)} com id = {id}")
        : Mapper.Map<Catalog>(entity);
    }

    public async Task<Catalog> GetByIdDetailedAsync(Guid id)
    {
        CatalogDao? entity = await Database
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id));

        return entity is null
            ? throw new Exception($"{nameof(CatalogDao)} com id = {id}")
        : Mapper.Map<Catalog>(entity);
    }
}
