using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class LastSearchedTagsConfiguration : IEntityTypeConfiguration<LastSearchedTagsDao>
{
	public void Configure(EntityTypeBuilder<LastSearchedTagsDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("last_searched_tags_pkey");

		entity.ToTable("last_searched_tags");
	}
}
