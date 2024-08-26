using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UExpo.Domain.Dao;
namespace UExpo.Repository.Configurations;

public class UserImageConfiguration : IEntityTypeConfiguration<UserImageDao>
{
    public void Configure(EntityTypeBuilder<UserImageDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("user_image_pkey");

        entity.ToTable("user_image");
    }
}
