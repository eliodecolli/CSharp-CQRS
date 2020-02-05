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
                var result = service.MarkShipmentAsDelivered(pack);
                if(result)
                {
                    // we REALLY need a "*" wildcard for searching queries in-memory
                    SystemCacheManager.RemoveQuery<GetAllShipmentsResponse>(new CachedQueryString("*", typeof(GetAllShipmentsQuery),
                                                                                    new CachedQueryParam("ShipmentId", pack.ShipmentId)));

                    SystemCacheManager.RemoveQuery<GetShipmentStatusResponse>(new CachedQueryString("*", typeof(GetShipmentStatusQuery),
                                                                                    new CachedQueryParam("ShipmentId", pack.ShipmentId)));
                    //SystemCacheManager.Reset();
                }
                return new ReaderConfirmation()
                {
                    Success = result,
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
