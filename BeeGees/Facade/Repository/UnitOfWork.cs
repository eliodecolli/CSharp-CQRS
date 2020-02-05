using BeeGees_WriteNode.Facade.Entities;
using System;

namespace BeeGees_WriteNode.Facade.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly BaseRepository<Shipment> shipmentsRepository;
        private readonly ShipmentContext context;

        public UnitOfWork()
        {
            this.context = new ShipmentContext();
            shipmentsRepository = new BaseRepository<Shipment>(context);
        }

        public BaseRepository<Shipment> ShipmentsRepository => shipmentsRepository;

        public void Dispose()
        {
            context.Dispose();   // keep it clean :)
            GC.SuppressFinalize(this);
        }

        public void SaveWork()
        {
            context.SaveChanges();
        }
    }
}
