using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class RelationshipConfiguration : IEntityTypeConfiguration<RelationshipDao>
{
	public void Configure(EntityTypeBuilder<RelationshipDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("relationship_pkey");

		entity.ToTable("relationship");

		entity
			.HasOne(r => r.BuyerUser)
			.WithMany(u => u.BuyerRelationships)
			.HasForeignKey(r => r.BuyerUserId)
			.OnDelete(DeleteBehavior.Cascade);

		entity
			.HasOne(r => r.SupplierUser)
			.WithMany(u => u.SupplierRelationships)
			.HasForeignKey(r => r.SupplierUserId)
			.OnDelete(DeleteBehavior.Cascade);

		entity
			.HasOne(r => r.Calendar)
			.WithMany(u => u.Relationships)
			.HasForeignKey(r => r.CalendarId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
