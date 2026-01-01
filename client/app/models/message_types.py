"""
Message types enum - matches the C# MessageType enum
"""
from enum import IntEnum


class MessageType(IntEnum):
    """Message type identifiers for RabbitMQ messages"""

    CREATE_NEW_SHIPMENT = 1
    UPDATE_SHIPMENT = 2
    MARK_SHIPMENT_AS_DELIVERED = 3
    SHIPMENT_CREATED_EVENT = 4
    SHIPMENT_UPDATED_EVENT = 5
    SHIPMENT_DELIVERED_EVENT = 6
    GET_ALL_SHIPMENTS_QUERY = 7
    GET_SHIPMENT_STATUS_QUERY = 8
