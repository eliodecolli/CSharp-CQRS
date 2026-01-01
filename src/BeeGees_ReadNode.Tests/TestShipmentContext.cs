using BeeGees_ReadNode.Facade.Entities;
using BeeGees_ReadNode.Facade.Repository;
using BeeGees_ReadNode.Facade.Service;
using Microsoft.EntityFrameworkCore;

namespace BeeGees_ReadNode.Tests
{
    public class TestShipmentContext : ShipmentContext
    {
        private readonly DbContextOptions<ShipmentContext> _options;

        public TestShipmentContext(DbContextOptions<ShipmentContext> options) : base()
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TestDatabase");
            }
        }
    }

    public class TestUnitOfWork : IDisposable
    {
        private readonly BaseRepository<Shipment> shipmentsRepository;
        private readonly TestShipmentContext context;

        public TestUnitOfWork(TestShipmentContext context)
        {
            this.context = context;
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

    public class TestableShipmentService : IDisposable
    {
        private readonly TestUnitOfWork unitOfWork;

        public TestableShipmentService(TestUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Shipment InsertNewFromEvent(BeeGees.Events.ShipmentCreatedEvent message)
        {
            try
            {
                var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

                if (ship != null)
                {
                    return null;
                }

                ship = unitOfWork.ShipmentsRepository.InsertNew(new Shipment()
                {
                    ShipmentId = Guid.Parse(message.ShipmentId),
                    Location = message.CurrentLocation,
                    CustomerId = message.CustomerId,
                    LastUpdated = DateTime.FromBinary(message.LastUpdated),
                    Status = message.Status,
                    ShipmentName = message.ShipmentName
                });

                unitOfWork.SaveWork();
                return ship;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdateShipmentFromEvent(BeeGees.Events.ShipmentUpdatedEvent message)
        {
            var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

            if (ship != null)
            {
                ship.Status = message.Status;
                ship.Location = message.Location;
                ship.LastUpdated = DateTime.FromBinary(message.LastUpdated);

                unitOfWork.ShipmentsRepository.Update(ship);
                unitOfWork.SaveWork();
                return true;
            }
            return false;
        }

        public (bool success, string customerId) MarkShipmentAsDelivered(BeeGees.Events.ShipmentDeliveredEvent message)
        {
            var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

            if(ship != null)
            {
                ship.Status = "Delivered";
                ship.LastUpdated = DateTime.FromBinary(message.DeliveredDate);

                unitOfWork.ShipmentsRepository.Update(ship);
                unitOfWork.SaveWork();
                return (true, ship.CustomerId);
            }

            return (false, string.Empty);
        }

        public BeeGees.Queries.Responses.GetAllShipmentsResponse GetAllShipments(BeeGees.Queries.GetAllShipmentsQuery query)
        {
            var retval = new BeeGees.Queries.Responses.GetAllShipmentsResponse();

            try
            {
                var ships = unitOfWork.ShipmentsRepository.Get(x => x.CustomerId == query.CustomerId);
                if(ships.Length > 0)
                {
                    retval.CustomerId = query.CustomerId;
                    retval.Shipments.AddRange(ships.Select(x => new BeeGees.Queries.Responses.Shipment()
                    {
                        CurrentLocation = x.Location,
                        LastStatusUpdate = x.LastUpdated.Ticks,
                        ShipmentName = x.ShipmentName,
                        ShipmentId = x.ShipmentId.ToString(),
                    }));
                    retval.Success = true;
                }
                else
                {
                    retval.Success = false;
                }
            }
            catch(Exception)
            {
                retval.Success = false;
            }

            return retval;
        }

        public BeeGees.Queries.Responses.GetShipmentStatusResponse GetShipmentStatus(BeeGees.Queries.GetShipmentStatusQuery query)
        {
            var retval = new BeeGees.Queries.Responses.GetShipmentStatusResponse();

            try
            {
                var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(query.ShipmentId));
                if(ship != null)
                {
                    retval.LastUpdated = ship.LastUpdated.Ticks;
                    retval.ShipmentName = ship.ShipmentName;
                    retval.Status = ship.Status;
                    retval.Success = true;
                }
                else
                {
                    retval.Success = false;
                }
            }
            catch (Exception)
            {
                retval.Success = false;
            }

            return retval;
        }
    }
}
