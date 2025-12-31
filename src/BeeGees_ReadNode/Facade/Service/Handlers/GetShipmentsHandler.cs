using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_Messaging.Handlers;
using BeeGees_ReadNode.Facade.FastAccess;
using Google.Protobuf;
using System.Diagnostics;
using System.Linq;

namespace BeeGees_ReadNode.Facade.Service.Handlers
{
    public class GetShipmentsHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var retval = new GetAllShipmentsResponse();

            if(message is GetAllShipmentsQuery)
            {
                var pack = (GetAllShipmentsQuery)message;

                if(SystemCacheManager.RegisterStatistic(pack.Sender, typeof(GetAllShipmentsQuery)))
                {
                    // we're having lots of stuff like this
                    var cachedQuery = new CachedQueryString(pack.Sender, typeof(GetAllShipmentsQuery), new CachedQueryParam("CustomerId", pack.CustomerId.ToString()));

                    var bestMatch = SystemCacheManager.RetrieveBestMatch<Shipment>(cachedQuery);
                    if (bestMatch != null && bestMatch.Count > 0)
                    {
                        retval.CustomerId = pack.CustomerId;
                        retval.Shipments.AddRange(bestMatch);
                        retval.Success = true;
                    }
                    else
                    {
                        // use the service
                        using var service = new ShipmentService();

                        retval = service.GetAllShipments(pack);

                        SystemCacheManager.RegisterQuery(cachedQuery, retval.Shipments.ToList());   // update our cache
                    }
                }
                else
                {
                    // well.. just use the service
                    using var service = new ShipmentService();

                    retval = service.GetAllShipments(pack);
                }
            }

            stopWatch.Stop();
            Log.Debug(" [x] Operation took " + stopWatch.ElapsedMilliseconds + "ms");

            return retval;
        }
    }
}
