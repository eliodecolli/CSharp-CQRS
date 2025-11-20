using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_WriteNode.Facade.Entities
{
    public class Shipment
    {
        public Guid ShipmentId { get; set; }

        public string ShipmentAddress { get; set; }

        public string ShipmentName { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public string CurrentStatus { get; set; }

        public string CurrentLocation { get; set; }

        public string CustomerId { get; set; }
    }
}
