using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class PanelConfiguration : IEntityTypeConfiguration<Panel>
    {
        public void Configure(EntityTypeBuilder<Panel> builder)
        {
            builder.ToTable("panels");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.PriceTotal)
                .HasColumnName("price_total")
                .IsRequired();

            builder.Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(x => x.Sales)
                .HasColumnName("sales")
                .IsRequired();

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Panel)
                .HasForeignKey<Panel>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.RegistrationClients)
                .WithOne(x => x.Panel)
                .HasForeignKey(x => x.PanelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}