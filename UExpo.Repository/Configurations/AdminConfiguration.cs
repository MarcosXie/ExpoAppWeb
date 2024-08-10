using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<AdminDao>
{
    public void Configure(EntityTypeBuilder<AdminDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("admin_pkey");

        entity.ToTable("admin");

        entity.HasIndex(x => x.Name).IsUnique();
    }
}
