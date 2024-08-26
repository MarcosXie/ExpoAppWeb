using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogItemImageRepository(UExpoDbContext context, IMapper mapper)
    : BaseRepository<CatalogItemImageDao, CatalogItemImage>(context, mapper), ICatalogItemImageRepository
{
    public async Task<List<CatalogItemImage>> GetMainImagesByCatalogAsync(Guid id)
    {
        var images = await Database
            .Where(x => x.CatalogId == id)
            .GroupBy(x => x.ItemId)
            .Select(g => g.OrderByDescending(x => x.Order).FirstOrDefault())    
            .ToListAsync();

        return images.Select(Mapper.Map<CatalogItemImage>).ToList();
    }

    public async Task<int> GetMaxOrderByProductAsync(string id)
    {
        var products = await Database.Where(x => x.ItemId == id).ToListAsync();
    
        return products.Count > 0 ? products.Max(x => x.Order) : 1;
    }
}
