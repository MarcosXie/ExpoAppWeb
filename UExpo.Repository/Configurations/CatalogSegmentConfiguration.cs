using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CatalogSegmentConfiguration : IEntityTypeConfiguration<CatalogSegmentDao>
{
	public void Configure(EntityTypeBuilder<CatalogSegmentDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("catalog_segment_pkey");

		entity.ToTable("catalog_segment");

		entity
			.HasOne(n => n.CalendarSegment)
			.WithMany(n => n.Catalogs)
			.HasForeignKey(n => n.CalendarSegmentId)
			.OnDelete(DeleteBehavior.Cascade);

		entity
			.HasOne(n => n.Catalog)
			.WithMany(n => n.Segments)
			.HasForeignKey(n => n.CatalogId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}