using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Catalogs.CatalogSegments;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogSegmentRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<CatalogSegmentDao, CatalogSegment>(context, mapper), ICatalogSegmentRepository
{
}

