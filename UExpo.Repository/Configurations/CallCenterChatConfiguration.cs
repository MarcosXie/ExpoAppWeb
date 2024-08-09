using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UExpo.Repository.Dao;

namespace UExpo.Repository.Configurations;

public class CallCenterChatConfiguration : IEntityTypeConfiguration<CallCenterChatDao>
{
    public void Configure(EntityTypeBuilder<CallCenterChatDao> entity)
    {
        entity.HasKey(x => x.Id).HasName("call_center_pkey");

        entity.ToTable("call_center_chat");

        entity
            .HasMany(n => n.Messages)
            .WithOne(n => n.CallCenterChat)
            .HasForeignKey(n => n.ChatId)
            .OnDelete(DeleteBehavior.Cascade);


        entity
            .HasOne(n => n.User)
            .WithOne(n => n.CallCenterChat)
            .OnDelete(DeleteBehavior.Cascade);

        entity
            .HasOne(n => n.Admin)
            .WithOne(n => n.CallCenterChat)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
