using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CallCenterMessageConfiguration : IEntityTypeConfiguration<CallCenterMessageDao>
{
    public void Configure(EntityTypeBuilder<CallCenterMessageDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("call_center_message_pkey");

        entity.ToTable("call_center_message");
    }
}
