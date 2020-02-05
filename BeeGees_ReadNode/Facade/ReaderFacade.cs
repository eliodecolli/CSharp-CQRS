using System;
using System.Collections.Generic;
using System.Text;
using BeeGees;
using BeeGees_Messaging;
using BeeGees_Messaging.Handlers;
using BeeGees_ReadNode.Facade.FastAccess;
using BeeGees_ReadNode.Facade.Service.Handlers;

namespace BeeGees_ReadNode.Facade
{
    public class ReaderFacade : IFacade
    {
        public IMessageHandler GenerateHandler(MessageType type)
        {
            switch(type)
            {
                case MessageType.GetAllShipmentsQuery:
                    return new GetShipmentsHandler();
                case MessageType.GetShipmentStatusQuery:
                    return new GetShipmentStatusHandler();
                case MessageType.ShipmentCreatedEvent:
                    return new ShipmentCreatedEventHandler();
                case MessageType.ShipmentDeliveredEvent:
                    return new ShipmentDeliveredEventHandler();
                case MessageType.ShipmentUpdatedEvent:
                    return new ShipmentUpdatedEventHandler();
                default:
                    return null;
            }
        }

        public void InitializeFastAccess(int threshold)
        {
            SystemCacheManager.Initialize(threshold);
        }
    }
}
