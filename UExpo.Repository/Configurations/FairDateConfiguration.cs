using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class FairDateConfiguration : IEntityTypeConfiguration<FairDateDao>
{
    public void Configure(EntityTypeBuilder<FairDateDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("fair_date_pkey");

        entity.ToTable("fair_date");
    }
}

