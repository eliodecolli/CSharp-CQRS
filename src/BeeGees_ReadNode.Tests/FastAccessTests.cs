using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees_ReadNode.Facade.FastAccess;
using Xunit;
using FastAccessClass = BeeGees_ReadNode.Facade.FastAccess.FastAccess;

namespace BeeGees_ReadNode.Tests
{
    public class FastAccessTests
    {
        [Fact]
        public void RegisterQuery_StoresDataInCache()
        {
            // Arrange
            var fastAccess = new FastAccessClass();
            var response = new GetAllShipmentsResponse
            {
                CustomerId = "customer1",
                Success = true
            };
            response.Shipments.Add(new BeeGees.Queries.Responses.Shipment
            {
                ShipmentId = "ship1",
                ShipmentName = "Package 1",
                CurrentLocation = "Warehouse"
            });

            var query = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));

            // Act
            fastAccess.RegisterQuery(query, response);
            var result = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("customer1", result[0].CustomerId);
        }

        [Fact]
        public void RetrieveBestMatch_ReturnsNullWhenNotCached()
        {
            // Arrange
            var fastAccess = new FastAccessClass();
            var query = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));

            // Act
            var result = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void RemoveQuery_InvalidatesCache()
        {
            // Arrange
            var fastAccess = new FastAccessClass();
            var response = new GetAllShipmentsResponse
            {
                CustomerId = "customer1",
                Success = true
            };
            response.Shipments.Add(new BeeGees.Queries.Responses.Shipment
            {
                ShipmentId = "ship1",
                ShipmentName = "Package 1",
                CurrentLocation = "Warehouse"
            });

            var query = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));

            fastAccess.RegisterQuery(query, response);

            // Act - Remove with wildcard sender
            var removeQuery = new CachedQueryString("*", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));
            fastAccess.RemoveQuery<GetAllShipmentsResponse>(removeQuery);

            // Verify cache is empty
            var result = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query);

            // Assert - After removal, the cache returns an empty list or null
            Assert.True(result == null || result.Count == 0);
        }

        [Fact]
        public void RegisterQuery_MultipleQueries_StoresSeparately()
        {
            // Arrange
            var fastAccess = new FastAccessClass();

            var response1 = new GetAllShipmentsResponse
            {
                CustomerId = "customer1",
                Success = true
            };
            response1.Shipments.Add(new BeeGees.Queries.Responses.Shipment
            {
                ShipmentId = "ship1",
                ShipmentName = "Package 1"
            });

            var response2 = new GetAllShipmentsResponse
            {
                CustomerId = "customer2",
                Success = true
            };
            response2.Shipments.Add(new BeeGees.Queries.Responses.Shipment
            {
                ShipmentId = "ship2",
                ShipmentName = "Package 2"
            });

            var query1 = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));
            var query2 = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer2"));

            // Act
            fastAccess.RegisterQuery(query1, response1);
            fastAccess.RegisterQuery(query2, response2);

            var result1 = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query1);
            var result2 = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query2);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Single(result1);
            Assert.Single(result2);
            Assert.Equal("customer1", result1[0].CustomerId);
            Assert.Equal("customer2", result2[0].CustomerId);
        }

        [Fact]
        public void RemoveQuery_OnlyInvalidatesMatchingCache()
        {
            // Arrange
            var fastAccess = new FastAccessClass();

            var response1 = new GetAllShipmentsResponse
            {
                CustomerId = "customer1",
                Success = true
            };
            var response2 = new GetAllShipmentsResponse
            {
                CustomerId = "customer2",
                Success = true
            };

            var query1 = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));
            var query2 = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer2"));

            fastAccess.RegisterQuery(query1, response1);
            fastAccess.RegisterQuery(query2, response2);

            // Act - Remove only customer1
            var removeQuery = new CachedQueryString("*", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));
            fastAccess.RemoveQuery<GetAllShipmentsResponse>(removeQuery);

            var result1 = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query1);
            var result2 = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query2);

            // Assert
            Assert.True(result1 == null || result1.Count == 0); // customer1 cache should be removed
            Assert.NotNull(result2); // customer2 cache should still exist
        }

        [Fact]
        public void ResetCache_ClearsAllData()
        {
            // Arrange
            var fastAccess = new FastAccessClass();

            var response = new GetAllShipmentsResponse
            {
                CustomerId = "customer1",
                Success = true
            };
            var query = new CachedQueryString("sender1", typeof(GetAllShipmentsQuery),
                new CachedQueryParam("CustomerId", "customer1"));

            fastAccess.RegisterQuery(query, response);

            // Act
            fastAccess.ResetCache();
            var result = fastAccess.RetrieveBestMatch<GetAllShipmentsResponse>(query);

            // Assert
            Assert.Null(result);
        }
    }
}
