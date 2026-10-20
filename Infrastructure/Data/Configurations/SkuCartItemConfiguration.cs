using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class SkuCartItemConfiguration : IEntityTypeConfiguration<SkuCartItem>
    {
        public void Configure(EntityTypeBuilder<SkuCartItem> builder)
        {
            builder.ToTable("sku_cart_item");

            // ID
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            // CART_ITEM_ID
            builder.Property(s => s.CartItemId)
                .HasColumnName("cart_item_id")
                .IsRequired();

            // SKU_PRODUCT_ID
            builder.Property(s => s.SkuId)
                .HasColumnName("sku_id")
                .IsRequired();

            // ACTIVE
            builder.Property(s => s.Active)
                .HasColumnName("active")
                .IsRequired();

            // SKU_CART_ITEM 1->1 CART_ITEM
            builder.HasOne(s => s.CartItem)
                .WithMany(ci => ci.SkuCartItens)
                .HasForeignKey(s => s.CartItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // SKU_CART_ITEM 1->1 SKU_PRODUCT
            builder.HasOne(s => s.SkuProduct)
                .WithMany(sk => sk.SkuCartItens)
                .HasForeignKey(s => s.SkuId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
