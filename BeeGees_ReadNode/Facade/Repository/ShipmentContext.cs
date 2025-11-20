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
            optionsBuilder.UseNpgsql("Host=localhost;Port=61660;Database=BeeGees_Reader;Username=postgres;Password=postgres");
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
