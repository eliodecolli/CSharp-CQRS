using BeeGees_Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using BeeGees.Events;
using Google.Protobuf;

namespace BeeGees_ReadNode.Facade.Service.Handlers
{
    public class ShipmentCreatedEventHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            if(message is ShipmentCreatedEvent)
            {
                var pack = (ShipmentCreatedEvent)message;

                using var service = new ShipmentService();
                var s = service.InsertNewFromEvent(pack);
                if(s != null)
                {
                    return new ReaderConfirmation()
                    {
                        Success = true,
                        Message = "Shipment created"
                    };
                }
            }
            Log.Error($"Why are you using ShipmentCreatedEvent to handle a {message.GetType().Name} message?");
            return new ReaderConfirmation()
            {
                Success = false,
                Message = "Wrong handler"
            };
        }
    }
}
