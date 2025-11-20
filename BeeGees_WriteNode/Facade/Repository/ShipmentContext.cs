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
            optionsBuilder.UseNpgsql("Host=localhost;Port=61660;Database=BeeGees_Writer;Username=postgres;Password=postgres");
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
