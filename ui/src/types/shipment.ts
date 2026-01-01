/**
 * TypeScript types for BeeGees Shipment API
 * Based on FastAPI backend schemas
 */

// Request Types
export interface CreateShipmentRequest {
  ship_name: string;
  ship_address: string;
  customer_id: string;
}

export interface UpdateShipmentLocationRequest {
  location: string;
  status: string;
}

export interface MarkShipmentDeliveredRequest {
  delivered_date?: string; // ISO datetime string
  additional_taxes: number; // in cents
}

// Response Types
export interface ShipmentResponse {
  shipment_id: string;
  shipment_name: string;
  current_location: string;
  last_status_update: string; // ISO datetime string
  status: string;
}

export interface CreateShipmentResponse {
  success: boolean;
  shipment_id: string;
}

export interface UpdateShipmentResponse {
  success: boolean;
  shipment_id: string;
  shipment_name: string;
}

export interface DeliverShipmentResponse {
  success: boolean;
  shipment_id: string;
}

export interface GetAllShipmentsResponse {
  customer_id: string;
  shipments: ShipmentResponse[];
  success: boolean;
}

export interface GetShipmentStatusResponse {
  shipment_name: string;
  status: string;
  last_updated: string; // ISO datetime string
  success: boolean;
}

export interface ErrorResponse {
  detail: string;
}
