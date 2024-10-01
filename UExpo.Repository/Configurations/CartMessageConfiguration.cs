using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CartMessageConfiguration : IEntityTypeConfiguration<CartMessageDao>
{
	public void Configure(EntityTypeBuilder<CartMessageDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("cart_message_pkey");

		entity.ToTable("cart_message");
	}
}