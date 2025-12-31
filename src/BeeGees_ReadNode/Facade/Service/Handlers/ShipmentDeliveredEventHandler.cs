using BeeGees_Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using BeeGees.Events;
using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_ReadNode.Facade.FastAccess;
using Google.Protobuf;

namespace BeeGees_ReadNode.Facade.Service.Handlers
{
    public class ShipmentDeliveredEventHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            if(message is ShipmentDeliveredEvent)
            {
                var pack = (ShipmentDeliveredEvent)message;

                using var service = new ShipmentService();
                var (success, customerId) = service.MarkShipmentAsDelivered(pack);

                if(success)
                {
                    // Invalidate cache for GetAllShipments queries for this customer
                    if (!string.IsNullOrEmpty(customerId))
                    {
                        SystemCacheManager.RemoveQuery<GetAllShipmentsResponse>(new CachedQueryString("*", typeof(GetAllShipmentsQuery),
                                                                                        new CachedQueryParam("CustomerId", customerId)));
                    }

                    // Invalidate cache for GetShipmentStatus queries for this shipment
                    SystemCacheManager.RemoveQuery<GetShipmentStatusResponse>(new CachedQueryString("*", typeof(GetShipmentStatusQuery),
                                                                                    new CachedQueryParam("ShipmentId", pack.ShipmentId)));
                }
                return new ReaderConfirmation()
                {
                    Success = success,
                    Message = ""
                };
            }
            Log.Error($"Why are you using ShipmentDeliveredEvent to handle a {message.GetType().Name} message?");
            return new ReaderConfirmation()
            {
                Success = false,
                Message = "Wrong handler."
            };
        }
    }
}
