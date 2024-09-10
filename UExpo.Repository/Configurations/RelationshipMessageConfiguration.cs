using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class RelationshipMessageConfiguration : IEntityTypeConfiguration<RelationshipMessageDao>
{
	public void Configure(EntityTypeBuilder<RelationshipMessageDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("relationship_message_pkey");

		entity.ToTable("relationship_message");
	}
}
