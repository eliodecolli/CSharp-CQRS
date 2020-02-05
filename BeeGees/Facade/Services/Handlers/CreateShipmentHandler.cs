using BeeGees.Commands;
using BeeGees.Commands.Responses;
using BeeGees_Messaging.Handlers;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_WriteNode.Facade.Services.Handlers
{
    public class CreateShipmentHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            var retval = new ShipmentCreatedResponse();
            using (var service = new ShipmentService())
            {
                retval = service.CreateShipment((CreateShipmentCommand)message);
            }
            return retval;
        }
    }
}
