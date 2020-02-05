using BeeGees_ReadNode.Facade.Entities;
using System;

namespace BeeGees_ReadNode.Facade.Repository
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
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public void SaveWork()
        {
            context.SaveChanges();
        }
    }
}
