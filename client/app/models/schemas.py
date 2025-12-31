"""
Pydantic models for API request/response schemas
"""
from datetime import datetime
from typing import List, Optional
from pydantic import BaseModel, Field


# Request Models
class CreateShipmentRequest(BaseModel):
    """Request to create a new shipment"""

    ship_name: str = Field(..., description="Name/description of the shipment")
    ship_address: str = Field(..., description="Delivery address")
    customer_id: str = Field(..., description="Customer identifier")

    class Config:
        json_schema_extra = {
            "example": {
                "ship_name": "Electronics Package",
                "ship_address": "123 Main St, New York, NY 10001",
                "customer_id": "CUST001",
            }
        }


class UpdateShipmentLocationRequest(BaseModel):
    """Request to update shipment location and status"""

    location: str = Field(..., description="New location of the shipment")
    status: str = Field(..., description="Status message")

    class Config:
        json_schema_extra = {
            "example": {
                "location": "Distribution Center - Chicago",
                "status": "Arrived at Distribution Center - Chicago",
            }
        }


class MarkShipmentDeliveredRequest(BaseModel):
    """Request to mark shipment as delivered"""

    delivered_date: Optional[datetime] = Field(
        default=None, description="Delivery date (defaults to now if not provided)"
    )
    additional_taxes: int = Field(
        default=0, description="Additional taxes or fees in cents"
    )

    class Config:
        json_schema_extra = {
            "example": {"delivered_date": "2025-01-15T14:30:00", "additional_taxes": 0}
        }


# Response Models
class ShipmentResponse(BaseModel):
    """Shipment information"""

    shipment_id: str = Field(..., description="Unique shipment identifier")
    shipment_name: str = Field(..., description="Name/description of the shipment")
    current_location: str = Field(..., description="Current location")
    last_status_update: datetime = Field(..., description="Last update timestamp")

    class Config:
        json_schema_extra = {
            "example": {
                "shipment_id": "SHP-20250115-001",
                "shipment_name": "Electronics Package",
                "current_location": "Distribution Center - Chicago",
                "last_status_update": "2025-01-15T14:30:00",
            }
        }


class CreateShipmentResponse(BaseModel):
    """Response from creating a shipment"""

    success: bool = Field(..., description="Whether the operation succeeded")
    shipment_id: str = Field(..., description="Created shipment ID")

    class Config:
        json_schema_extra = {"example": {"success": True, "shipment_id": "SHP-20250115-001"}}


class UpdateShipmentResponse(BaseModel):
    """Response from updating a shipment"""

    success: bool = Field(..., description="Whether the operation succeeded")
    shipment_id: str = Field(..., description="Updated shipment ID")
    shipment_name: str = Field(..., description="Shipment name")

    class Config:
        json_schema_extra = {
            "example": {
                "success": True,
                "shipment_id": "SHP-20250115-001",
                "shipment_name": "Electronics Package",
            }
        }


class DeliverShipmentResponse(BaseModel):
    """Response from marking shipment as delivered"""

    success: bool = Field(..., description="Whether the operation succeeded")
    shipment_id: str = Field(..., description="Delivered shipment ID")

    class Config:
        json_schema_extra = {"example": {"success": True, "shipment_id": "SHP-20250115-001"}}


class GetAllShipmentsResponse(BaseModel):
    """Response containing all shipments for a customer"""

    customer_id: str = Field(..., description="Customer ID")
    shipments: List[ShipmentResponse] = Field(..., description="List of shipments")
    success: bool = Field(..., description="Whether the operation succeeded")

    class Config:
        json_schema_extra = {
            "example": {
                "customer_id": "CUST001",
                "shipments": [
                    {
                        "shipment_id": "SHP-20250115-001",
                        "shipment_name": "Electronics Package",
                        "current_location": "Distribution Center - Chicago",
                        "last_status_update": "2025-01-15T14:30:00",
                    }
                ],
                "success": True,
            }
        }


class GetShipmentStatusResponse(BaseModel):
    """Response containing shipment status"""

    shipment_name: str = Field(..., description="Shipment name")
    status: str = Field(..., description="Current status")
    last_updated: datetime = Field(..., description="Last update timestamp")
    success: bool = Field(..., description="Whether the operation succeeded")

    class Config:
        json_schema_extra = {
            "example": {
                "shipment_name": "Electronics Package",
                "status": "Arrived at Distribution Center - Chicago",
                "last_updated": "2025-01-15T14:30:00",
                "success": True,
            }
        }


class ErrorResponse(BaseModel):
    """Error response"""

    detail: str = Field(..., description="Error message")

    class Config:
        json_schema_extra = {"example": {"detail": "Shipment not found"}}
