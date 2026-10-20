using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class SkuMovementConfiguration : IEntityTypeConfiguration<SkuMovement>
    {
        public void Configure(EntityTypeBuilder<SkuMovement> builder)
        {
            builder.ToTable("sku_movement");

            // ID
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            // SKU_PRODUCT_ID
            builder.Property(s => s.SkuProductId)
                .HasColumnName("sku_product_id")
                .IsRequired();

            // MOVEMENT_ID
            builder.Property(s => s.MovementId)
                .HasColumnName("movement_id")
                .IsRequired();

            // SKU_MOVEMENT 1->1 SKU_PRODUCT
            builder.HasOne(s => s.SkuProduct)
                .WithMany(k => k.SkuMovements)
                .HasForeignKey(k => k.SkuProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // SKU_MOVEMENT 1->1 MOVEMENT
            builder.HasOne(s => s.Movement)
                .WithMany(m => m.SkuMovements)
                .HasForeignKey(s => s.MovementId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
