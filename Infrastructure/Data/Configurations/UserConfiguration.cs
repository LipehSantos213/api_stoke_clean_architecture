using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api_stoke.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            // PK
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            // Name
            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(120)
                .IsRequired();

            // Email
            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(200)
                .IsRequired();

            // Email único
            builder.HasIndex(x => x.Email)
                .IsUnique();

            // Password
            builder.Property(x => x.Password)
                .HasColumnName("password")
                .IsRequired();

            // Enum
            builder.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Active
            builder.Property(x => x.Active)
                .HasColumnName("active")
                .IsRequired();

            // Datas
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            // User -> Products
            builder.HasMany(x => x.Products)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User -> Panel
            builder.HasOne(x => x.Panel)
                .WithOne(x => x.User)
                .HasForeignKey<Panel>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Cart
            builder.HasOne(x => x.Cart)
                .WithOne(x => x.User)
                .HasForeignKey<Cart>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}