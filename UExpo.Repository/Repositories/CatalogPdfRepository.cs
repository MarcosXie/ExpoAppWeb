using AutoMapper;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Dao;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogPdfRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogPdfDao, CatalogPdf>(context, mapper), ICatalogPdfRepository
{
}
