using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CalendarFairConfiguration : IEntityTypeConfiguration<CalendarFairDao>
{
    public void Configure(EntityTypeBuilder<CalendarFairDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("calendar_fair_pkey");

        entity.ToTable("calendar_fair");
    }
}
