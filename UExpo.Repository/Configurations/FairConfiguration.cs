using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UExpo.Domain.Dao;
namespace UExpo.Repository.Configurations;

public class FairConfiguration : IEntityTypeConfiguration<FairDao>
{
    public void Configure(EntityTypeBuilder<FairDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("fair_pkey");

        entity.ToTable("fair");
    }
}
