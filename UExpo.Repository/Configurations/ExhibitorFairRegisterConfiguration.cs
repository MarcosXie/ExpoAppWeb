using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UExpo.Domain.Dao;
namespace UExpo.Repository.Configurations;

public class ExhibitorFairRegisterConfiguration : IEntityTypeConfiguration<ExhibitorFairRegisterDao>
{
    public void Configure(EntityTypeBuilder<ExhibitorFairRegisterDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("exhibitor_fair_register_pkey");

        entity.ToTable("exhibitor_fair_register");

        entity
            .HasOne(x => x.User)
            .WithMany(x => x.FairRegisters)
            .HasForeignKey(x => x.ExhibitorId)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasOne(x => x.CalendarFair)
            .WithMany(x => x.FairRegisters)
            .HasForeignKey(x => x.CalendarFairId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
