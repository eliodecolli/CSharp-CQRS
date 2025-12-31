using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_Messaging
{
    public enum MessageType : int
    {
        CreateNewShipment = 1,
        UpdateShipment,
        MarkShipmentAsDelivered,
        ShipmentCreatedEvent,
        ShipmentUpdatedEvent,
        ShipmentDeliveredEvent,
        GetAllShipmentsQuery,
        GetShipmentStatusQuery
    }
}
