using AutoMapper;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogPdfRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogPdfDao, CatalogPdf>(context, mapper), ICatalogPdfRepository
{
}
