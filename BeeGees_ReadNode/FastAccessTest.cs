using BeeGees_ReadNode.Facade.FastAccess;
using BeeGees_ReadNode.Facade.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;

using BE = BeeGees.Queries.Responses;

namespace BeeGees_ReadNode
{
    public class FastAccessTest
    {
        public void Test()
        {
            // create random entities
            var shipmentsForUser = new List<Shipment>();

            for (int i = 0; i < 200; i++)
            {
                shipmentsForUser.Add(new Shipment()
                {
                    CustomerId = "1",
                    LastUpdated = DateTime.Now,
                    Location = "Ship @ " + i.ToString(),
                    ShipmentId = Guid.NewGuid(),
                    ShipmentName = "Shipment #" + (i + 1).ToString(),
                    Status = "On its way!"
                });
            }
            // test FastAccessFacade
            SystemCacheManager.Initialize(10);

            // first add some basic data
            // Shipment/CustomerId=1/
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SystemCacheManager.RegisterQuery(new CachedQueryString("localhost", typeof(Shipment), new CachedQueryParam("CustomerId", "1")), shipmentsForUser); // at /localhost/Shipment/CustomerId=1/ should be a list of 10 shipments :)
            SystemCacheManager.RegisterQuery(new CachedQueryString("192.168.0.1", typeof(Shipment), new CachedQueryParam("CustomerId", "1")), shipmentsForUser); // at /localhost/Shipment/CustomerId=1/ should be a list of 10 shipments :)

            stopwatch.Stop();
            Console.WriteLine($"Adding 20k items in " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Reset();

            // try to get Mary...

            //stopwatch.Start();
            //var maryClone = SystemCacheManager.RetrieveBestMatch<Shipment>(new CachedQueryString("localhost", typeof(Shipment), new CachedQueryParam("CustomerId", mary.CustomerId.ToString()),
            //                                                                              new CachedQueryParam("ShipmentName", mary.ShipmentName))).First();
            //stopwatch.Stop();
            //Console.WriteLine(maryClone.ShipmentName);
            //Console.WriteLine("Mary took " + stopwatch.ElapsedMilliseconds + "ms");
            //stopwatch.Reset();


            // try to get shipment with ID 1
            stopwatch.Start();
            var a = SystemCacheManager.RetrieveBestMatch<Shipment>(new CachedQueryString("*", typeof(Shipment), new CachedQueryParam("CustomerId", "1")));
            stopwatch.Stop();
            Console.WriteLine("Ship1 took " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}
