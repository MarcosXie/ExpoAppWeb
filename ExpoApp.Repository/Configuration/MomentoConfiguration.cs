using ExpoApp.Domain.Dao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpoApp.Repository.Configuration;

public class MomentoConfiguration : IEntityTypeConfiguration<MomentoDao>
{
    public void Configure(EntityTypeBuilder<MomentoDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("momento_pkey");

        entity.ToTable("momento");
        
        entity
	        .HasOne(x => x.User)
	        .WithMany()
	        .HasForeignKey(x => x.UserId)
	        .OnDelete(DeleteBehavior.Cascade);
    }
}
