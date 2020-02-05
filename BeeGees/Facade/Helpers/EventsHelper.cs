using System;
using System.IO;
using BeeGees;
using BeeGees.Commands.Responses;
using BeeGees.Events;
using BeeGees_Messaging;
using BeeGees_WriteNode.Facade.Repository;
using Google.Protobuf;

namespace BeeGees_WriteNode.Facade.Helpers
{
    internal static class EventsHelper
    {
        public static BaseMessage GenerateEventMessageFromResponse(object response)
        {
            var retval = new BaseMessage();

            using (var unitOfWork = new UnitOfWork())
            {
                if (response is ShipmentCreatedResponse)
                {
                    var bl = (ShipmentCreatedResponse)response;

                    var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(bl.ShipmentId));
                    var fact = new ShipmentCreatedEvent()
                    {
                        CurrentLocation = ship.CurrentLocation,
                        CustomerId = ship.CustomerId,
                        LastUpdated = ship.LastUpdated.ToBinary(),
                        ShipmentId = ship.ShipmentId.ToString(),
                        ShipmentAddress = ship.ShipmentAddress,
                        Status = ship.CurrentStatus,
                        ShipmentName = ship.ShipmentName
                    };
                    retval.Type = (int)MessageType.ShipmentCreatedEvent;
                    retval.Blob = fact.ToByteString();
                }
                else if(response is ShipmentUpdatedResponse)
                {
                    var bl = (ShipmentUpdatedResponse)response;

                    var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(bl.ShipmentId));
                    var fact = new ShipmentUpdatedEvent()
                    {
                        ShipmentId = ship.ShipmentId.ToString(),
                        Status = ship.CurrentStatus,
                        Location = ship.CurrentLocation,
                        LastUpdated = ship.LastUpdated.ToBinary(),
                        CustomerId = ship.CustomerId
                    };

                    retval.Type = (int)MessageType.ShipmentUpdatedEvent;
                    retval.Blob = fact.ToByteString();
                }
                else if(response is ShipmentDeliveredResponse)
                {
                    var bl = (ShipmentDeliveredResponse)response;

                    var ship = unitOfWork.ShipmentsRepository.Find(Guid.Parse(bl.ShipmentId));
                    var fact = new ShipmentDeliveredEvent()
                    {
                        ShipmentId = ship.ShipmentId.ToString(),
                        DeliveredDate = ship.DeliveredAt.HasValue ? ship.DeliveredAt.Value.ToBinary() : -1
                    };
                    retval.Type = (int)MessageType.ShipmentDeliveredEvent;
                    retval.Blob = ByteString.CopyFrom(fact.ToByteArray());
                }
            }

            return retval;
        }
    }
}
