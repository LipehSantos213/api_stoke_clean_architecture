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
                .ValueGeneratedNever();

            builder.Property(x => x.PanelId)
                .HasColumnName("panel_id")
                .IsRequired();

            builder.Property(x => x.MovementId)
                .HasColumnName("movement_id")
                .IsRequired();

            builder.Property(x => x.CartId)
                .HasColumnName("cart_id")
                .IsRequired();

            builder.HasOne(x => x.Panel)
                .WithMany(x => x.RegistrationClients)
                .HasForeignKey(x => x.PanelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Movement)
                .WithOne(x => x.RegistrationClient)
                .HasForeignKey<RegistrationClients>(x => x.MovementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cart)
                .WithMany(x => x.RegistrationClients)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.MovementId)
                .IsUnique();
        }
    }
}