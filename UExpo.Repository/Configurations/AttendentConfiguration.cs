using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Configurations;

public class AttendentConfiguration : IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("attendent_pkey");

        entity.ToTable("attendent");
    }
}
