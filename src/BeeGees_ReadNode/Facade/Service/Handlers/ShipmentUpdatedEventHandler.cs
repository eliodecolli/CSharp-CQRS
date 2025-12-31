using BeeGees_Messaging.Handlers;
using BeeGees.Events;
using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_ReadNode.Facade.FastAccess;
using Google.Protobuf;

namespace BeeGees_ReadNode.Facade.Service.Handlers
{
    public class ShipmentUpdatedEventHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            if(message is ShipmentUpdatedEvent)
            {
                var pack = (ShipmentUpdatedEvent)message;

                using var service = new ShipmentService();
                var result = service.UpdateShipmentFromEvent(pack);

                if(result)
                {
                    SystemCacheManager.RemoveQuery<GetAllShipmentsResponse>(new CachedQueryString("*", typeof(GetAllShipmentsQuery),
                                                                                    new CachedQueryParam("CustomerId", pack.CustomerId)));

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

            Log.Error($"Why are you using ShipmentUpdatedEvent to handle a {message.GetType().Name} message?");
            return new ReaderConfirmation()
            {
                Success = false,
                Message = "Wrong handler."
            };
        }
    }
}
