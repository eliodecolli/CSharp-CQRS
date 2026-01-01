/**
 * API Service for BeeGees Shipment Tracking System
 * Communicates with FastAPI backend
 */
import axios from 'axios';
import type { AxiosInstance } from 'axios';
import type {
  CreateShipmentRequest,
  CreateShipmentResponse,
  UpdateShipmentLocationRequest,
  UpdateShipmentResponse,
  MarkShipmentDeliveredRequest,
  DeliverShipmentResponse,
  GetAllShipmentsResponse,
  GetShipmentStatusResponse,
} from '../types/shipment';

// API configuration
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:8000';

class ShipmentApiService {
  private client: AxiosInstance;

  constructor() {
    this.client = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
      timeout: 30000, // 30 seconds timeout
    });

    // Response interceptor for error handling
    this.client.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response) {
          // Server responded with error status
          throw new Error(error.response.data.detail || 'An error occurred');
        } else if (error.request) {
          // Request made but no response
          throw new Error('No response from server. Please check your connection.');
        } else {
          // Error in request setup
          throw new Error('Failed to make request');
        }
      }
    );
  }

  /**
   * Create a new shipment
   */
  async createShipment(data: CreateShipmentRequest): Promise<CreateShipmentResponse> {
    const response = await this.client.post<CreateShipmentResponse>('/api/shipments', data);
    return response.data;
  }

  /**
   * Get all shipments for a customer
   */
  async getAllShipments(customerId: string): Promise<GetAllShipmentsResponse> {
    const response = await this.client.get<GetAllShipmentsResponse>('/api/shipments', {
      params: { customer_id: customerId },
    });
    return response.data;
  }

  /**
   * Get status of a specific shipment
   */
  async getShipmentStatus(shipmentId: string): Promise<GetShipmentStatusResponse> {
    const response = await this.client.get<GetShipmentStatusResponse>(`/api/shipments/${shipmentId}`);
    return response.data;
  }

  /**
   * Update shipment location and status
   */
  async updateShipmentLocation(
    shipmentId: string,
    data: UpdateShipmentLocationRequest
  ): Promise<UpdateShipmentResponse> {
    const response = await this.client.put<UpdateShipmentResponse>(
      `/api/shipments/${shipmentId}/location`,
      data
    );
    return response.data;
  }

  /**
   * Mark shipment as delivered
   */
  async markShipmentDelivered(
    shipmentId: string,
    data: MarkShipmentDeliveredRequest
  ): Promise<DeliverShipmentResponse> {
    const response = await this.client.post<DeliverShipmentResponse>(
      `/api/shipments/${shipmentId}/deliver`,
      data
    );
    return response.data;
  }
}

// Export singleton instance
export const shipmentApi = new ShipmentApiService();
