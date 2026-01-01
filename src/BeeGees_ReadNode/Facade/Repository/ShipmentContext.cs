using BeeGees_ReadNode.Facade.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeeGees_ReadNode.Facade.Repository
{
    public class ShipmentContext : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }

        public ShipmentContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "61660";
            var database = Environment.GetEnvironmentVariable("DB_NAME") ?? "BeeGees_Reader";
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
                .Property(x => x.ShipmentId)
                .ValueGeneratedNever();

            modelBuilder.Entity<Shipment>()
                .ToTable("Read_Shipments");

            base.OnModelCreating(modelBuilder);
        }
    }
}
