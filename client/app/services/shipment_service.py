"""
Shipment service for handling business logic using generated protobuf classes
"""
import logging
from datetime import datetime
from typing import List

from app.services.rabbitmq_service import rabbitmq_service
from app.models.message_types import MessageType
from app.models.schemas import (
    ShipmentResponse,
    CreateShipmentResponse,
    UpdateShipmentResponse,
    DeliverShipmentResponse,
    GetAllShipmentsResponse,
    GetShipmentStatusResponse,
)

# Import generated protobuf classes
from app.proto_generated.BaseMessage_pb2 import BaseMessage
from app.proto_generated.Commands.CreateShipmentCommand_pb2 import CreateShipmentCommand
from app.proto_generated.Commands.UpdateShipmentStatusCommand_pb2 import UpdateShipmentCommand
from app.proto_generated.Commands.MarkShipmentAsDeliveredCommand_pb2 import MarkShipmentAsDeliveredCommand
from app.proto_generated.Commands.Responses.ShipmentCreatedResponse_pb2 import ShipmentCreatedResponse as ProtoShipmentCreatedResponse
from app.proto_generated.Commands.Responses.ShipmentUpdatedResponse_pb2 import ShipmentUpdatedResponse as ProtoShipmentUpdatedResponse
from app.proto_generated.Commands.Responses.ShipmentDeliveredResponse_pb2 import ShipmentDeliveredResponse as ProtoShipmentDeliveredResponse
from app.proto_generated.Queries.GetAllShipmentsQuery_pb2 import GetAllShipmentsQuery
from app.proto_generated.Queries.GetShipmentStatusQuery_pb2 import GetShipmentStatusQuery
from app.proto_generated.Queries.Responses.GetAllShipmentsResponse_pb2 import GetAllShipmentsResponse as ProtoGetAllShipmentsResponse
from app.proto_generated.Queries.Responses.GetShipmentStatusResponse_pb2 import GetShipmentStatusResponse as ProtoGetShipmentStatusResponse

logger = logging.getLogger(__name__)


class ShipmentService:
    """Service for shipment operations using protobuf messages"""

    @staticmethod
    def _datetime_to_dotnet_binary(dt: datetime) -> int:
        """Convert Python datetime to .NET DateTime.ToBinary() format"""
        # .NET DateTime ticks are 100-nanosecond intervals since 0001-01-01 00:00:00
        epoch = datetime(1, 1, 1)
        delta = dt - epoch
        ticks = int(delta.total_seconds() * 10_000_000)
        return ticks

    @staticmethod
    def _dotnet_binary_to_datetime(ticks: int) -> datetime:
        """Convert .NET DateTime.ToBinary() format to Python datetime"""
        from datetime import timedelta
        epoch = datetime(1, 1, 1)
        # Each tick is 100 nanoseconds = 0.0000001 seconds
        seconds = ticks / 10_000_000.0
        return epoch + timedelta(seconds=seconds)

    async def create_shipment(
        self, ship_name: str, ship_address: str, customer_id: str
    ) -> CreateShipmentResponse:
        """Create a new shipment"""
        logger.info(f"Creating shipment: {ship_name} for customer {customer_id}")

        # Build CreateShipmentCommand
        command = CreateShipmentCommand()
        command.ShipName = ship_name
        command.ShipAddress = ship_address
        command.CustomerID = customer_id

        # Wrap in BaseMessage
        base_message = BaseMessage()
        base_message.Type = MessageType.CREATE_NEW_SHIPMENT
        base_message.Blob = command.SerializeToString()

        # Send command
        response_data = await rabbitmq_service.send_command(base_message.SerializeToString())

        # Parse response
        proto_response = ProtoShipmentCreatedResponse()
        proto_response.ParseFromString(response_data)

        return CreateShipmentResponse(
            success=proto_response.Success,
            shipment_id=proto_response.ShipmentId,
        )

    async def update_shipment(
        self, shipment_id: str, location: str, status: str
    ) -> UpdateShipmentResponse:
        """Update shipment location and status"""
        logger.info(f"Updating shipment {shipment_id} to location: {location}")

        # Build UpdateShipmentCommand
        command = UpdateShipmentCommand()
        command.ShipmentId = shipment_id
        command.Status = status
        command.Location = location

        # Wrap in BaseMessage
        base_message = BaseMessage()
        base_message.Type = MessageType.UPDATE_SHIPMENT
        base_message.Blob = command.SerializeToString()

        # Send command
        response_data = await rabbitmq_service.send_command(base_message.SerializeToString())

        # Parse response
        proto_response = ProtoShipmentUpdatedResponse()
        proto_response.ParseFromString(response_data)

        return UpdateShipmentResponse(
            success=proto_response.Success,
            shipment_id=proto_response.ShipmentId,
            shipment_name=proto_response.ShipmentName,
        )

    async def mark_as_delivered(
        self, shipment_id: str, delivered_date: datetime, additional_taxes: int
    ) -> DeliverShipmentResponse:
        """Mark shipment as delivered"""
        logger.info(f"Marking shipment {shipment_id} as delivered")

        # Build MarkShipmentAsDeliveredCommand
        command = MarkShipmentAsDeliveredCommand()
        command.ShipmentId = shipment_id
        command.DeliveredDate = self._datetime_to_dotnet_binary(delivered_date)
        command.AdditionalTaxes = additional_taxes

        # Wrap in BaseMessage
        base_message = BaseMessage()
        base_message.Type = MessageType.MARK_SHIPMENT_AS_DELIVERED
        base_message.Blob = command.SerializeToString()

        # Send command
        response_data = await rabbitmq_service.send_command(base_message.SerializeToString())

        # Parse response
        proto_response = ProtoShipmentDeliveredResponse()
        proto_response.ParseFromString(response_data)

        return DeliverShipmentResponse(
            success=proto_response.Success,
            shipment_id=proto_response.ShipmentId,
        )

    async def get_all_shipments(self, customer_id: str) -> GetAllShipmentsResponse:
        """Get all shipments for a customer"""
        logger.info(f"Getting all shipments for customer {customer_id}")

        # Build GetAllShipmentsQuery
        query = GetAllShipmentsQuery()
        query.CustomerId = customer_id
        query.Sender = "python-api"  # For caching optimization

        # Wrap in BaseMessage
        base_message = BaseMessage()
        base_message.Type = MessageType.GET_ALL_SHIPMENTS_QUERY
        base_message.Blob = query.SerializeToString()

        # Send query
        response_data = await rabbitmq_service.send_query(base_message.SerializeToString())

        # Parse response
        proto_response = ProtoGetAllShipmentsResponse()
        proto_response.ParseFromString(response_data)

        # Convert protobuf shipments to our schema
        shipments = []
        for proto_shipment in proto_response.Shipments:
            shipments.append(
                ShipmentResponse(
                    shipment_id=proto_shipment.ShipmentId,
                    shipment_name=proto_shipment.ShipmentName,
                    current_location=proto_shipment.CurrentLocation,
                    last_status_update=self._dotnet_binary_to_datetime(
                        proto_shipment.LastStatusUpdate
                    ),
                    status=proto_shipment.Status,
                )
            )

        return GetAllShipmentsResponse(
            customer_id=proto_response.CustomerId,
            shipments=shipments,
            success=proto_response.Success,
        )

    async def get_shipment_status(self, shipment_id: str) -> GetShipmentStatusResponse:
        """Get status of a specific shipment"""
        logger.info(f"Getting status for shipment {shipment_id}")

        # Build GetShipmentStatusQuery
        query = GetShipmentStatusQuery()
        query.ShipmentId = shipment_id
        query.Sender = "python-api"

        # Wrap in BaseMessage
        base_message = BaseMessage()
        base_message.Type = MessageType.GET_SHIPMENT_STATUS_QUERY
        base_message.Blob = query.SerializeToString()

        # Send query
        response_data = await rabbitmq_service.send_query(base_message.SerializeToString())

        # Parse response
        proto_response = ProtoGetShipmentStatusResponse()
        proto_response.ParseFromString(response_data)

        return GetShipmentStatusResponse(
            shipment_name=proto_response.ShipmentName,
            status=proto_response.Status,
            last_updated=self._dotnet_binary_to_datetime(proto_response.LastUpdated),
            success=proto_response.Success,
        )


# Global instance
shipment_service = ShipmentService()
