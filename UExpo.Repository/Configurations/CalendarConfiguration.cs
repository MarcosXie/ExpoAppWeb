using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CalendarConfiguration : IEntityTypeConfiguration<CalendarDao>
{
    public void Configure(EntityTypeBuilder<CalendarDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("calendar_pkey");

        entity.ToTable("calendar");
    }
}
