using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CatalogPdfConfiguration : IEntityTypeConfiguration<CatalogPdfDao>
{
    public void Configure(EntityTypeBuilder<CatalogPdfDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("catalog_pdf_pkey");

        entity.ToTable("catalog_pdf");
    }
}
