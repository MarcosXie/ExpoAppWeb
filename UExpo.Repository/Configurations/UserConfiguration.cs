using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("user_pkey");

        entity.ToTable("user");
    }
}
