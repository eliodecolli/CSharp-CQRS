using System;
using System.Collections.Generic;
using System.Text;

namespace BeeGees_ReadNode.Facade.Entities
{
    public class Shipment
    {
        public Guid ShipmentId { get; set; }

        public string ShipmentName { get; set; }

        public string CustomerId { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
