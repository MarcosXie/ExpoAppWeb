using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CatalogConfiguration : IEntityTypeConfiguration<CatalogDao>
{
    public void Configure(EntityTypeBuilder<CatalogDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("catalog_pkey");

        //entity
        //    .Property(e => e.JsonTable)
        //    .HasColumnType("string");

        entity
            .HasMany(n => n.ItemImages)
            .WithOne(n => n.Catalog)
            .HasForeignKey(n => n.CatalogId)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasMany(n => n.Pdfs)
            .WithOne(n => n.Catalog)
            .HasForeignKey(n => n.CatalogId)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasOne(n => n.User)
            .WithOne(n => n.Catalog)
            .OnDelete(DeleteBehavior.Cascade);

        entity.ToTable("catalog");
    }
}
