using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class RegistrationClientsConfiguration
        : IEntityTypeConfiguration<RegistrationClients>
    {
        public void Configure(
            EntityTypeBuilder<RegistrationClients> builder)
        {
            builder.ToTable("registration_clients");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(x => x.PanelId)
                .HasColumnName("panel_id")
                .IsRequired();

            builder.Property(x => x.MovementId)
                .HasColumnName("movement_id")
                .IsRequired();
            builder.HasIndex(x => x.MovementId)
                .IsUnique();

            builder.Property(x => x.CartId)
                .HasColumnName("cart_id")
                .IsRequired();

            builder.HasOne(rc => rc.Panel)
                .WithMany(p => p.RegistrationClients)
                .HasForeignKey(rc => rc.PanelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Movement)
                .WithOne(m => m.RegistrationClient)
                .HasForeignKey<RegistrationClients>(rc => rc.MovementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rc => rc.Cart)
                .WithMany(c => c.RegistrationClients)
                .HasForeignKey(rc => rc.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(rc => new
            {
                rc.PanelId,
                rc.CartId
            });
        }
    }
}