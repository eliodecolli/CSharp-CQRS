using BeeGees.Events;
using BeeGees.Queries;
using BeeGees_ReadNode.Facade.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BeeGees_ReadNode.Tests
{
    [Collection("Sequential")]
    public class ShipmentServiceTests : IDisposable
    {
        private readonly TestShipmentContext _context;
        private readonly TestUnitOfWork _unitOfWork;
        private readonly TestableShipmentService _service;

        public ShipmentServiceTests()
        {
            var options = new DbContextOptionsBuilder<ShipmentContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TestShipmentContext(options);
            _unitOfWork = new TestUnitOfWork(_context);
            _service = new TestableShipmentService(_unitOfWork);
        }

        public void Dispose()
        {
            _service.Dispose();
            _unitOfWork.Dispose();
            _context.Dispose();
        }

        [Fact]
        public void InsertNewFromEvent_ValidEvent_CreatesShipment()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var createEvent = new ShipmentCreatedEvent
            {
                ShipmentId = shipmentId.ToString(),
                CustomerId = "customer1",
                ShipmentName = "Test Package",
                ShipmentAddress = "123 Test St",
                CurrentLocation = "Warehouse",
                Status = "Processing",
                LastUpdated = DateTime.Now.ToBinary()
            };

            // Act
            var result = _service.InsertNewFromEvent(createEvent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(shipmentId, result.ShipmentId);
            Assert.Equal("Test Package", result.ShipmentName);
            Assert.Equal("customer1", result.CustomerId);
            Assert.Equal("Processing", result.Status);
        }

        [Fact]
        public void UpdateShipmentFromEvent_ValidEvent_UpdatesShipment()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var createEvent = new ShipmentCreatedEvent
            {
                ShipmentId = shipmentId.ToString(),
                CustomerId = "customer1",
                ShipmentName = "Test Package",
                CurrentLocation = "Warehouse",
                Status = "Processing",
                LastUpdated = DateTime.Now.ToBinary()
            };
            _service.InsertNewFromEvent(createEvent);

            var updateEvent = new ShipmentUpdatedEvent
            {
                ShipmentId = shipmentId.ToString(),
                CustomerId = "customer1",
                Status = "In Transit",
                Location = "Distribution Center",
                LastUpdated = DateTime.Now.ToBinary()
            };

            // Act
            var result = _service.UpdateShipmentFromEvent(updateEvent);

            // Assert
            Assert.True(result);
            var shipment = _unitOfWork.ShipmentsRepository.Find(shipmentId);
            Assert.Equal("In Transit", shipment.Status);
            Assert.Equal("Distribution Center", shipment.Location);
        }

        [Fact]
        public void MarkShipmentAsDelivered_ValidEvent_MarksAsDelivered()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var createEvent = new ShipmentCreatedEvent
            {
                ShipmentId = shipmentId.ToString(),
                CustomerId = "customer1",
                ShipmentName = "Test Package",
                CurrentLocation = "Warehouse",
                Status = "Processing",
                LastUpdated = DateTime.Now.ToBinary()
            };
            _service.InsertNewFromEvent(createEvent);

            var deliverEvent = new ShipmentDeliveredEvent
            {
                ShipmentId = shipmentId.ToString(),
                DeliveredDate = DateTime.Now.ToBinary()
            };

            // Act
            var (success, customerId) = _service.MarkShipmentAsDelivered(deliverEvent);

            // Assert
            Assert.True(success);
            Assert.Equal("customer1", customerId);
            var shipment = _unitOfWork.ShipmentsRepository.Find(shipmentId);
            Assert.Equal("Delivered", shipment.Status);
        }

        [Fact]
        public void GetShipmentStatus_ReturnsCorrectStatus()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var createEvent = new ShipmentCreatedEvent
            {
                ShipmentId = shipmentId.ToString(),
                CustomerId = "customer1",
                ShipmentName = "Test Package",
                CurrentLocation = "Warehouse",
                Status = "Processing",
                LastUpdated = DateTime.Now.ToBinary()
            };
            _service.InsertNewFromEvent(createEvent);

            var query = new GetShipmentStatusQuery
            {
                ShipmentId = shipmentId.ToString()
            };

            // Act
            var result = _service.GetShipmentStatus(query);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Test Package", result.ShipmentName);
            Assert.Equal("Processing", result.Status);
        }
    }
}
