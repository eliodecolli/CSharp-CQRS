using BeeGees.Commands;
using BeeGees_Messaging.Handlers;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_WriteNode.Facade.Services.Handlers
{
    public class UpdateShipmentHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            IMessage retval = null;

            using (var service = new ShipmentService())
            {
                if (message is MarkShipmentAsDeliveredCommand)
                {
                    retval = service.MarkDelivery((MarkShipmentAsDeliveredCommand)message);
                }
                else if (message is UpdateShipmentCommand)
                {
                    retval = service.UpdateShipment((UpdateShipmentCommand)message);
                }
            }

            return retval;
        }
    }
}
