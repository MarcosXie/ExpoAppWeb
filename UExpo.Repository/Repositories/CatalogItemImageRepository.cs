using AutoMapper;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogItemImageRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogItemImageDao, CatalogItemImage>(context, mapper), ICatalogItemImageRepository
{
}
