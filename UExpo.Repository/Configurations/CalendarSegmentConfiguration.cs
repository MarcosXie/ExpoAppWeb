using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CalendarSegmentConfiguration : IEntityTypeConfiguration<CalendarSegmentDao>
{
    public void Configure(EntityTypeBuilder<CalendarSegmentDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("calendar_segment_pkey");

        entity.ToTable("calendar_segment");
    }
}
