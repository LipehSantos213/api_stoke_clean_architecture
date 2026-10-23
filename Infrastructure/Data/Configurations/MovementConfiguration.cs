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

            builder.HasMany(m => m.SkuMovements)
                .WithOne(sm => sm.Movement)
                .HasForeignKey(sm => sm.MovementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.RegistrationClient)
                .WithOne(rc => rc.Movement)
                .HasForeignKey<RegistrationClients>(rc=> rc.MovementId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}