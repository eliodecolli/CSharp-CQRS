using Microsoft.EntityFrameworkCore;
using BeeGees_WriteNode.Facade.Entities;

namespace BeeGees_WriteNode.Facade.Repository
{
    public class ShipmentContext : DbContext
    {
        public ShipmentContext()
        {
        }

        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "61660";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "BeeGees_Writer";
            var username = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

            var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .HasKey(x => x.ShipmentId);

            modelBuilder.Entity<Shipment>()
                .ToTable("Write_Shipments");

            base.OnModelCreating(modelBuilder);
        }
    }
}
