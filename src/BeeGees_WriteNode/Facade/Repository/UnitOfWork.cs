using BeeGees_WriteNode.Facade.Entities;

namespace BeeGees_WriteNode.Facade.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IRepository<Shipment> shipmentsRepository;
        private readonly ShipmentContext context;

        public UnitOfWork()
        {
            this.context = new ShipmentContext();
            shipmentsRepository = new BaseRepository<Shipment>(context);
        }

        public IRepository<Shipment> ShipmentsRepository => shipmentsRepository;

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
