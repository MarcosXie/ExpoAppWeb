using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class AgendaConfiguration : IEntityTypeConfiguration<AgendaDao>
{
    public void Configure(EntityTypeBuilder<AgendaDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("agenda_pkey");

        entity.ToTable("agenda");
    }
}

