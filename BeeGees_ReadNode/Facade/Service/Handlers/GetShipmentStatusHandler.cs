using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_Messaging.Handlers;
using BeeGees_ReadNode.Facade.FastAccess;
using Google.Protobuf;
using System.Linq;

namespace BeeGees_ReadNode.Facade.Service.Handlers
{
    public class GetShipmentStatusHandler : IMessageHandler
    {
        public IMessage HandleMessage(object message)
        {
            var retval = new GetShipmentStatusResponse();

            if (message is GetShipmentStatusQuery)
            {
                var pack = (GetShipmentStatusQuery)message;
                 
                if (SystemCacheManager.RegisterStatistic(pack.Sender, typeof(GetShipmentStatusQuery)))
                {
                    // we're having lots of stuff like this
                    var cachedQuery = new CachedQueryString(pack.Sender, typeof(GetShipmentStatusQuery), new CachedQueryParam("ShipmentId", pack.ShipmentId));

                    var bestMatch = SystemCacheManager.RetrieveBestMatch<GetShipmentStatusResponse>(cachedQuery);
                    if (bestMatch != null && bestMatch.Count > 0)
                    {
                        // just get the first one
                        retval = bestMatch.First();
                    }
                    else
                    {
                        // use the service
                        using var service = new ShipmentService();

                        retval = service.GetShipmentStatus(pack);

                        SystemCacheManager.RegisterQuery(cachedQuery, retval);   // update our cache
                    }
                }
                else
                {
                    // well.. just use the service
                    using var service = new ShipmentService();

                    retval = service.GetShipmentStatus(pack);
                }
            }

            return retval;
        }
    }
}
