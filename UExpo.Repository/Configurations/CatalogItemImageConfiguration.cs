using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CatalogItemImagesConfiguration : IEntityTypeConfiguration<CatalogItemImageDao>
{
    public void Configure(EntityTypeBuilder<CatalogItemImageDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("catalog_item_image_pkey");

        entity.ToTable("catalog_item_image");
    }
}
