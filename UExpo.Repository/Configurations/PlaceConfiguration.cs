using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<PlaceDao>
{
    public void Configure(EntityTypeBuilder<PlaceDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("place_pkey");

        entity.ToTable("place");

        entity.HasIndex(x => x.Name).IsUnique();
    }
}

