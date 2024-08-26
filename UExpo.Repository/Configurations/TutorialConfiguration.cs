using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class TutorialConfiguration : IEntityTypeConfiguration<TutorialDao>
{
    public void Configure(EntityTypeBuilder<TutorialDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("tutorial_pkey");

        entity.ToTable("tutorial");
    }
}
