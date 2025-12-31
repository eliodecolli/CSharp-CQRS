"""
Shipment API endpoints
"""
import logging
from datetime import datetime
from typing import List

from fastapi import APIRouter, HTTPException, status, Query

from app.models.schemas import (
    CreateShipmentRequest,
    CreateShipmentResponse,
    UpdateShipmentLocationRequest,
    UpdateShipmentResponse,
    MarkShipmentDeliveredRequest,
    DeliverShipmentResponse,
    GetAllShipmentsResponse,
    GetShipmentStatusResponse,
)
from app.services.shipment_service import shipment_service

logger = logging.getLogger(__name__)

router = APIRouter(prefix="/api/shipments", tags=["shipments"])


@router.post(
    "",
    response_model=CreateShipmentResponse,
    status_code=status.HTTP_201_CREATED,
    summary="Create a new shipment",
    description="Create a new shipment in the system. Returns the created shipment ID.",
)
async def create_shipment(request: CreateShipmentRequest) -> CreateShipmentResponse:
    """Create a new shipment"""
    try:
        result = await shipment_service.create_shipment(
            ship_name=request.ship_name,
            ship_address=request.ship_address,
            customer_id=request.customer_id,
        )

        if not result.success:
            raise HTTPException(
                status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
                detail="Failed to create shipment",
            )

        return result

    except TimeoutError as e:
        logger.error(f"Timeout creating shipment: {e}")
        raise HTTPException(
            status_code=status.HTTP_504_GATEWAY_TIMEOUT, detail="Request timed out"
        )
    except Exception as e:
        logger.error(f"Error creating shipment: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to create shipment: {str(e)}",
        )


@router.get(
    "",
    response_model=GetAllShipmentsResponse,
    summary="Get all shipments",
    description="Retrieve all shipments for a specific customer.",
)
async def get_all_shipments(
    customer_id: str = Query(..., description="Customer ID to filter shipments")
) -> GetAllShipmentsResponse:
    """Get all shipments for a customer"""
    try:
        result = await shipment_service.get_all_shipments(customer_id=customer_id)

        if not result.success:
            raise HTTPException(
                status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
                detail="Failed to retrieve shipments",
            )

        return result

    except TimeoutError as e:
        logger.error(f"Timeout getting shipments: {e}")
        raise HTTPException(
            status_code=status.HTTP_504_GATEWAY_TIMEOUT, detail="Request timed out"
        )
    except Exception as e:
        logger.error(f"Error getting shipments: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to retrieve shipments: {str(e)}",
        )


@router.get(
    "/{shipment_id}",
    response_model=GetShipmentStatusResponse,
    summary="Get shipment status",
    description="Retrieve the current status and information for a specific shipment.",
)
async def get_shipment_status(shipment_id: str) -> GetShipmentStatusResponse:
    """Get status of a specific shipment"""
    try:
        result = await shipment_service.get_shipment_status(shipment_id=shipment_id)

        if not result.success:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND, detail="Shipment not found"
            )

        return result

    except TimeoutError as e:
        logger.error(f"Timeout getting shipment status: {e}")
        raise HTTPException(
            status_code=status.HTTP_504_GATEWAY_TIMEOUT, detail="Request timed out"
        )
    except Exception as e:
        logger.error(f"Error getting shipment status: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to retrieve shipment status: {str(e)}",
        )


@router.put(
    "/{shipment_id}/location",
    response_model=UpdateShipmentResponse,
    summary="Update shipment location",
    description="Update the location and status of a shipment as it moves through delivery.",
)
async def update_shipment_location(
    shipment_id: str, request: UpdateShipmentLocationRequest
) -> UpdateShipmentResponse:
    """Update shipment location and status"""
    try:
        result = await shipment_service.update_shipment(
            shipment_id=shipment_id,
            location=request.location,
            status=request.status,
        )

        if not result.success:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Shipment not found or update failed",
            )

        return result

    except TimeoutError as e:
        logger.error(f"Timeout updating shipment: {e}")
        raise HTTPException(
            status_code=status.HTTP_504_GATEWAY_TIMEOUT, detail="Request timed out"
        )
    except Exception as e:
        logger.error(f"Error updating shipment: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to update shipment: {str(e)}",
        )


@router.post(
    "/{shipment_id}/deliver",
    response_model=DeliverShipmentResponse,
    summary="Mark shipment as delivered",
    description="Mark a shipment as delivered with optional delivery date and additional charges.",
)
async def mark_shipment_delivered(
    shipment_id: str, request: MarkShipmentDeliveredRequest
) -> DeliverShipmentResponse:
    """Mark shipment as delivered"""
    try:
        delivered_date = request.delivered_date or datetime.now()

        result = await shipment_service.mark_as_delivered(
            shipment_id=shipment_id,
            delivered_date=delivered_date,
            additional_taxes=request.additional_taxes,
        )

        if not result.success:
            raise HTTPException(
                status_code=status.HTTP_404_NOT_FOUND,
                detail="Shipment not found or delivery marking failed",
            )

        return result

    except TimeoutError as e:
        logger.error(f"Timeout marking shipment as delivered: {e}")
        raise HTTPException(
            status_code=status.HTTP_504_GATEWAY_TIMEOUT, detail="Request timed out"
        )
    except Exception as e:
        logger.error(f"Error marking shipment as delivered: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Failed to mark shipment as delivered: {str(e)}",
        )


@router.get(
    "/health",
    summary="Health check",
    description="Check if the API is running",
    include_in_schema=False,
)
async def health_check():
    """Health check endpoint"""
    return {"status": "healthy", "service": "BeeGees Shipment API"}
