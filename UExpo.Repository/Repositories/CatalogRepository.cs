using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Exceptions;
using UExpo.Repository.Context;

namespace UExpo.Repository.Repositories;

public class CatalogRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<CatalogDao, Catalog>(context, mapper), ICatalogRepository
{
    public async Task<Catalog?> GetByUserIdOrDefaultAsync(Guid id)
    {
        CatalogDao? catalog = await Database
            .Include(x => x.Pdfs)
            .FirstOrDefaultAsync(x => x.UserId == id);

        return catalog is null ? null : Mapper.Map<Catalog>(catalog);
    }

	public async Task<Catalog> GetByCartIdAsync(Guid cartId)
	{
		CatalogDao? catalog = await Database
			.Include(x => x.Pdfs)
			.FirstOrDefaultAsync(x => x.User.SupplierCarts.Any(x => x.Id == cartId))
			?? throw new NotFoundException("Cart");

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
                Tags = x.Tags,
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

    public async Task<List<CatalogItemImage>> GetImagesByProductIdAsync(Guid id, string productId)
    {
        var images = await Context.CatalogImages
            .Where(x => x.CatalogId == id && x.ItemId == productId).ToListAsync();

        return images.Select(Mapper.Map<CatalogItemImage>).ToList();
    }

    public async Task UpdateTagsAsync(Catalog catalog)
    {
        await Context.Catalogs
            .Where(x => x.Id == catalog.Id)
            .ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Tags, catalog.Tags));
    }

	public async Task<List<string>> GetAllTagsAsync()
	{
		var tempTags = await Database
			.Select(x => x.Tags)
			.ToListAsync();

		List<string> tags = [];

		foreach (var tag in tempTags)
		{
			tags.AddRange(tag.Split(','));
		}

		return [.. tags.Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x)];
	}
}
