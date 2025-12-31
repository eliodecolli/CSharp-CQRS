using BeeGees.Commands;
using BeeGees_WriteNode.Facade.Entities;
using BeeGees_WriteNode.Facade.Repository;
using BeeGees_WriteNode.Facade.Services;
using Moq;
using Xunit;

namespace BeeGees_WriteNode.Tests.Services
{
    public class ShipmentServiceTests
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<IRepository<Shipment>> mockRepository;

        public ShipmentServiceTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockRepository = new Mock<IRepository<Shipment>>();
            mockUnitOfWork.Setup(x => x.ShipmentsRepository).Returns(mockRepository.Object);
        }

        [Fact]
        public void CreateShipment_ValidCommand_ReturnsSuccessWithShipmentId()
        {
            // Arrange
            var command = new CreateShipmentCommand
            {
                CustomerID = "CUST123",
                ShipName = "Test Package",
                ShipAddress = "123 Test St"
            };

            var createdShipment = new Shipment
            {
                ShipmentId = Guid.NewGuid(),
                CustomerId = command.CustomerID,
                ShipmentName = command.ShipName,
                ShipmentAddress = command.ShipAddress,
                CurrentLocation = "Base",
                CurrentStatus = "Package Received"
            };

            mockRepository.Setup(x => x.InsertNew(It.IsAny<Shipment>()))
                .Returns(createdShipment);

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.CreateShipment(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(createdShipment.ShipmentId.ToString(), result.ShipmentId);
            mockRepository.Verify(x => x.InsertNew(It.IsAny<Shipment>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveWork(), Times.Once);
        }

        [Fact]
        public void CreateShipment_RepositoryThrowsException_ReturnsFailure()
        {
            // Arrange
            var command = new CreateShipmentCommand
            {
                CustomerID = "CUST123",
                ShipName = "Test Package",
                ShipAddress = "123 Test St"
            };

            mockRepository.Setup(x => x.InsertNew(It.IsAny<Shipment>()))
                .Throws(new Exception("Database error"));

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.CreateShipment(command);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void UpdateShipment_ValidCommand_ReturnsSuccessWithUpdatedInfo()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var command = new UpdateShipmentCommand
            {
                ShipmentId = shipmentId.ToString(),
                Status = "In Transit",
                Location = "Distribution Center"
            };

            var existingShipment = new Shipment
            {
                ShipmentId = shipmentId,
                ShipmentName = "Test Package",
                CustomerId = "CUST123",
                CurrentStatus = "Package Received",
                CurrentLocation = "Base"
            };

            mockRepository.Setup(x => x.Find(shipmentId))
                .Returns(existingShipment);

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.UpdateShipment(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(shipmentId.ToString(), result.ShipmentId);
            Assert.Equal(existingShipment.ShipmentName, result.ShipmentName);
            Assert.Equal(existingShipment.CustomerId, result.CustomerId);
            mockRepository.Verify(x => x.Update(It.IsAny<Shipment>()), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveWork(), Times.Once);
        }

        [Fact]
        public void UpdateShipment_InvalidShipmentId_ReturnsFailure()
        {
            // Arrange
            var command = new UpdateShipmentCommand
            {
                ShipmentId = Guid.NewGuid().ToString(),
                Status = "In Transit",
                Location = "Distribution Center"
            };

            mockRepository.Setup(x => x.Find(It.IsAny<Guid>()))
                .Returns((Shipment)null!);

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.UpdateShipment(command);

            // Assert
            Assert.False(result.Success);
            mockRepository.Verify(x => x.Update(It.IsAny<Shipment>()), Times.Never);
        }

        [Fact]
        public void UpdateShipment_RepositoryThrowsException_ReturnsFailure()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var command = new UpdateShipmentCommand
            {
                ShipmentId = shipmentId.ToString(),
                Status = "In Transit",
                Location = "Distribution Center"
            };

            mockRepository.Setup(x => x.Find(It.IsAny<Guid>()))
                .Throws(new Exception("Database error"));

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.UpdateShipment(command);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public void MarkDelivery_ValidCommand_ReturnsSuccessWithExtraFees()
        {
            // Arrange
            var shipmentId = Guid.NewGuid();
            var deliveredDate = DateTime.UtcNow;
            var command = new MarkShipmentAsDeliveredCommand
            {
                ShipmentId = shipmentId.ToString(),
                DeliveredDate = deliveredDate.ToBinary(),
                AdditionalTaxes = 15
            };

            var existingShipment = new Shipment
            {
                ShipmentId = shipmentId,
                ShipmentName = "Test Package",
                ShipmentAddress = "123 Test St",
                CurrentStatus = "In Transit",
                CurrentLocation = "Near Destination"
            };

            mockRepository.Setup(x => x.Find(shipmentId))
                .Returns(existingShipment);

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.MarkDelivery(command);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(shipmentId.ToString(), result.ShipmentId);
            Assert.Equal(command.AdditionalTaxes, result.ExtraFees);
            mockUnitOfWork.Verify(x => x.SaveWork(), Times.Once);
        }

        [Fact]
        public void MarkDelivery_InvalidShipmentId_ReturnsFailure()
        {
            // Arrange
            var command = new MarkShipmentAsDeliveredCommand
            {
                ShipmentId = Guid.NewGuid().ToString(),
                DeliveredDate = DateTime.UtcNow.ToBinary(),
                AdditionalTaxes = 10
            };

            mockRepository.Setup(x => x.Find(It.IsAny<Guid>()))
                .Returns((Shipment)null!);

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.MarkDelivery(command);

            // Assert
            Assert.False(result.Success);
            mockUnitOfWork.Verify(x => x.SaveWork(), Times.Never);
        }

        [Fact]
        public void MarkDelivery_RepositoryThrowsException_ReturnsFailure()
        {
            // Arrange
            var command = new MarkShipmentAsDeliveredCommand
            {
                ShipmentId = Guid.NewGuid().ToString(),
                DeliveredDate = DateTime.UtcNow.ToBinary(),
                AdditionalTaxes = 10
            };

            mockRepository.Setup(x => x.Find(It.IsAny<Guid>()))
                .Throws(new Exception("Database error"));

            using var service = new ShipmentService(mockUnitOfWork.Object);

            // Act
            var result = service.MarkDelivery(command);

            // Assert
            Assert.False(result.Success);
        }
    }
}
