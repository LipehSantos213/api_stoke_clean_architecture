
using api_stoke.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace api_stoke.Infrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Representar a Tabela Users no banco PostgresSQL
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Panel> Panels { get; set; } = null!;

        public DbSet<Movement> Movements { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;

        public DbSet<RegistrationClients> RegistrationClients { get; set; } = null!;

        public DbSet<CartItem> CartItens { get; set; } = null!;
             
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                    typeof(AppDbContext).Assembly
                );
        }
    }
}
