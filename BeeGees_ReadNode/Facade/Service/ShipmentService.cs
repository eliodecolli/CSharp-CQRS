using BeeGees.Events;
using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_ReadNode.Facade.Repository;
using Google.Protobuf.Collections;
using System;
using System.Linq;
using Vk = BeeGees.Queries.Responses.Shipment;

namespace BeeGees_ReadNode.Facade.Service
{
    public class ShipmentService : IDisposable
    {
        private readonly UnitOfWork unitOfWork;

        public ShipmentService()
            => unitOfWork = new UnitOfWork();

        public void Dispose()
        {
            unitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }

        public GetAllShipmentsResponse GetAllShipments(GetAllShipmentsQuery query)
        {
            var retval = new GetAllShipmentsResponse();

            try
            {
                var ships = unitOfWork.ShipmentsRepository.Get(x => x.CustomerId == query.CustomerId);
                if(ships.Length > 0)
                {
                    retval.CustomerId = query.CustomerId;
                    var rp = new RepeatedField<Vk>();

                    retval.Shipments.AddRange(ships.Select(x => new Vk()
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
                    Log.Info($" [x] No entries found for Customer Id {query.CustomerId}");
                    retval.Success = false;
                }
            }
            catch(Exception ex)
            {
                Log.Error($" [x] Something, something, something, DARK SIDEEE {ex.Message}");
                retval.Success = false;
            }

            return retval;
        }

        public GetShipmentStatusResponse GetShipmentStatus(GetShipmentStatusQuery query)
        {
            var retval = new GetShipmentStatusResponse();

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
                    Log.Info($" [x] No entries found for Shipment with Id {query.ShipmentId}");
                }
            }
            catch (Exception ex)
            {
                retval.Success = false;
                Log.Error($" [x] GET TO THE CHOPPAH {ex.Message}");
            }

            return retval;
        }

        public bool UpdateShipmentFromEvent(ShipmentUpdatedEvent message)
        {
            var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

            if (ship != null)
            {
                ship.Status = message.Status;
                ship.Location = message.Location;
                ship.LastUpdated = DateTime.FromBinary(message.LastUpdated);

                unitOfWork.ShipmentsRepository.Update(ship);
                unitOfWork.SaveWork();

                Log.Info($" [x] Shipment {message.ShipmentId} has been updated");
                return true;
            }
            Log.Error($" [x] Cannot update shipment {message.ShipmentId}: id has not been found in our records.");
            return false;
        }

        public Entities.Shipment InsertNewFromEvent(ShipmentCreatedEvent message)
        {
            try
            {
                var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

                if (ship != null)
                {
                    Log.Error($"Cannot insert a new shipment with id {message.ShipmentId}: key is already present.");
                    return null;
                }

                ship = unitOfWork.ShipmentsRepository.InsertNew(new Entities.Shipment()
                {
                    ShipmentId = Guid.Parse(message.ShipmentId),
                    Location = message.CurrentLocation,
                    CustomerId = message.CustomerId,
                    LastUpdated = DateTime.FromBinary(message.LastUpdated),
                    Status = message.Status,
                    ShipmentName = message.ShipmentName
                });

                unitOfWork.SaveWork();
                Log.Info($"Inserted new shipment {message.ShipmentName} with id {message.ShipmentId}");

                return ship;
            }
            catch (Exception ex)
            {
                Log.Error($" [x] Something wrong happened while adding a new ship: {ex.Message}");
                return null;
            }
        }

        public bool MarkShipmentAsDelivered(ShipmentDeliveredEvent message)
        {
            var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(message.ShipmentId));

            if(ship != null)
            {
                ship.Status = "Delivered";
                ship.LastUpdated = DateTime.FromBinary(message.DeliveredDate);

                unitOfWork.ShipmentsRepository.Update(ship);
                unitOfWork.SaveWork();

                Log.Info($" [x] Shipment {ship.ShipmentName} with id {ship.ShipmentId} has been delivered");
                return true;
            }

            Log.Error($" [x] Cannot update shipment: id {message.ShipmentId} not found");
            return false;
        }
    }
}
