using BeeGees_WriteNode.Facade.Entities;
using BeeGees.Commands;
using BeeGees.Commands.Responses;
using BeeGees_WriteNode.Facade.Repository;
using System;

namespace BeeGees_WriteNode.Facade.Services
{
    public class ShipmentService : IDisposable
    {
        private readonly UnitOfWork unitOfWork;

        public ShipmentService()
            => unitOfWork = new UnitOfWork();

        public ShipmentCreatedResponse CreateShipment(CreateShipmentCommand command)
        {
            var retval = new ShipmentCreatedResponse();

            try
            {
                var nShipment = new Shipment()
                {
                    CurrentLocation = "Base",
                    CurrentStatus = "Package Received",
                    CustomerId = command.CustomerID,
                    LastUpdated = DateTime.UtcNow,
                    ShipmentName = command.ShipName,
                    ShipmentAddress = command.ShipAddress,
                    ShipmentId = Guid.NewGuid(),
                    DeliveredAt = null
                };

                nShipment = unitOfWork.ShipmentsRepository.InsertNew(nShipment);
                unitOfWork.SaveWork();

                retval.ShipmentId = nShipment.ShipmentId.ToString();
                retval.Success = true;
                Log.Info($"Created shipment {command.ShipName} on {nShipment.LastUpdated} en route to {command.ShipAddress}");
            }
            catch(Exception ex)
            {
                Log.Error($" [x] Something wrong happened while creating a new shipment! {ex.Message}");
                retval.Success = false;
            }
            return retval;
        }

        public ShipmentUpdatedResponse UpdateShipment(UpdateShipmentCommand command)
        {
            var retval = new ShipmentUpdatedResponse();
            Log.Info(" [x] -> UpdateShipment()");
            try
            {
                var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(command.ShipmentId));
                if(ship == null)
                {
                    retval.Success = false;
                    Log.Error($" [x] Invalid shipment id provided {command.ShipmentId}");
                }
                else
                {
                    ship.CurrentStatus = command.Status;
                    ship.CurrentLocation = command.Location;
                    ship.LastUpdated = DateTime.UtcNow;

                    unitOfWork.ShipmentsRepository.Update(ship);
                    unitOfWork.SaveWork();

                    retval.ShipmentName = ship.ShipmentName;
                    retval.Success = true;
                    retval.ShipmentId = ship.ShipmentId.ToString();
                    retval.CustomerId = ship.CustomerId;
                    Log.Info($" [x] {ship.ShipmentName} has been updated");
                }
            }
            catch (Exception ex)
            {
                retval.Success = false;
                Log.Error($" [x] Something wrong happened while updating shipment {command.ShipmentId}: {ex.Message}");
            }

            return retval;
        }

        public ShipmentDeliveredResponse MarkDelivery(MarkShipmentAsDeliveredCommand command)
        {
            var retval = new ShipmentDeliveredResponse();

            try
            {
                var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(command.ShipmentId));

                if (ship != null)
                {
                    ship.LastUpdated = DateTime.UtcNow;
                    ship.CurrentLocation = ship.ShipmentAddress;
                    ship.DeliveredAt = DateTime.FromBinary(command.DeliveredDate);

                    // contact the "Accouting Service" and update the needed taxes for the shipment

                    retval.Success = true;
                    retval.ShipmentId = ship.ShipmentId.ToString();
                    retval.ExtraFees = command.AdditionalTaxes;
                    unitOfWork.SaveWork();
                    Log.Info($" [x] Shipment {ship.ShipmentName} has been delivered");
                }
                else
                {
                    Log.Error(" [x] Couldn't find shipment with id " + command.ShipmentId);
                    retval.Success = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($" [x] Something went wrong while marking a shipment as delivered! {ex.Message} - {ex.InnerException?.Message}");
                retval.Success = false;
            }

            return retval;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
