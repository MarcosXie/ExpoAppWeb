using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;

namespace UExpo.Repository.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<CartDao>
{
	public void Configure(EntityTypeBuilder<CartDao> entity)
	{
		entity.HasKey(x => x.Id).HasName("cart_pkey");

		entity.ToTable("cart");

		entity
			.HasOne(r => r.BuyerUser)
			.WithMany(u => u.BuyerCarts)
			.HasForeignKey(r => r.BuyerUserId)
			.OnDelete(DeleteBehavior.Cascade);

		entity
			.HasOne(r => r.SupplierUser)
			.WithMany(u => u.SupplierCarts)
			.HasForeignKey(r => r.SupplierUserId)
			.OnDelete(DeleteBehavior.Cascade);

		entity
			.HasMany(r => r.Items)
			.WithOne(u => u.Cart)
			.HasForeignKey(r => r.CartId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}

