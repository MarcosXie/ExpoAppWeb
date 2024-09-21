using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItemDao>
{
	public void Configure(EntityTypeBuilder<CartItemDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("cart_item_pkey");

		entity.ToTable("cart_item");
	}
}