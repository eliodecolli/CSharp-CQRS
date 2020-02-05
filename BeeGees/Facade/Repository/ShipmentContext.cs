using System;
using System.Collections.Generic;
using System.Data.Entity;
using BeeGees_WriteNode.Facade.Entities;

namespace BeeGees_WriteNode.Facade.Repository
{
    public class ShipmentContext : DbContext
    {

        public ShipmentContext() : base("Server=localhost;Database=BeeGees_Writer;Trusted_Connection=True;") {


        }

        public DbSet<Shipment> Shipments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shipment>()
                .HasKey(x => x.ShipmentId)
                .ToTable("Write_Shipments");

            base.OnModelCreating(modelBuilder);
        }
    }
}
