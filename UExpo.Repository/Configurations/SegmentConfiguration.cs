using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class SegmentConfiguration : IEntityTypeConfiguration<SegmentDao>
{
    public void Configure(EntityTypeBuilder<SegmentDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("segment_pkey");

        entity.ToTable("segment");
    }
}
