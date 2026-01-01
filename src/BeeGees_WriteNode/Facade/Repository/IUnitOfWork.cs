using BeeGees_WriteNode.Facade.Entities;

namespace BeeGees_WriteNode.Facade.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Shipment> ShipmentsRepository { get; }
        void SaveWork();
    }
}
