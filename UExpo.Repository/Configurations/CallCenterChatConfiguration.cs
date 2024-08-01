using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Configurations;

public class CallCenterChatConfiguration : IEntityTypeConfiguration<CallCenterChatDao>
{
    public void Configure(EntityTypeBuilder<CallCenterChatDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("call_center_pkey");

        entity.ToTable("call_center_chat", "uexpo_db");

        entity
            .HasMany(n => n.Messages)
            .WithOne(n => n.CallCenterChatDao)
            .HasForeignKey(n => n.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
