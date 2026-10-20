using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class SkuProductConfiguration: IEntityTypeConfiguration<SkuProduct>
    {
        public void Configure(EntityTypeBuilder<SkuProduct> builder)
        {
            builder.ToTable("sku_products");

            // ID
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            // ProductId
            builder.Property(s => s.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            // SKU
            builder.Property(s => s.Sku)
                .HasColumnName("sku")
                .HasMaxLength(10)
                .IsRequired();
            builder.HasIndex(s => s.Sku)
                .IsUnique();

            // Active
            builder.Property(s => s.Active)
                .HasColumnName("active")
                .IsRequired();

            // Status
            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(12)
                .IsRequired();

            // SkuProduct -> Product
            builder.HasOne(s => s.Product)
                .WithMany(p => p.Skus)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // SkuProduct -> SkuMoements
            builder.HasMany(s => s.SkuMovements)
                .WithOne(m=>m.SkuProduct).
                HasForeignKey(m=>m.SkuProductId).
                OnDelete(DeleteBehavior.Restrict); // Se esse SKU for deletado
                                                  // como fica m.SkuProduct
        }
    }
}