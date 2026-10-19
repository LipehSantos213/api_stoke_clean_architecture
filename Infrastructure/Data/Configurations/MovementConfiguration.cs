using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.ToTable("movements");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.TypeMovement)
                .HasColumnName("type_movement")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Observation)
                .HasColumnName("observation")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.User)
                .HasColumnName("user")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.HasOne(m => m.Product)
                .WithMany(p => p.Movements)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}