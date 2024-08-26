using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UExpo.Domain.Dao;
namespace UExpo.Repository.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("user_pkey");

        entity.ToTable("user");

        entity
            .HasMany(x => x.Images)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
