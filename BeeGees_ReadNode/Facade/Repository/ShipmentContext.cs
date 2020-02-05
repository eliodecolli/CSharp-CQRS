using BeeGees_ReadNode.Facade.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BeeGees_ReadNode.Facade.Repository
{
    public class ShipmentContext : DbContext
    {
        public DbSet<Shipment> Shipments { get; set; }

        public ShipmentContext() : base("Server=localhost;Database=BeeGees_Reader;Trusted_Connection=True;") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .HasKey(x => x.ShipmentId)
                .Property(x => x.ShipmentId).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
            modelBuilder.Entity<Shipment>()
                .ToTable("Read_Shipments");

            base.OnModelCreating(modelBuilder);
        }
    }
}
