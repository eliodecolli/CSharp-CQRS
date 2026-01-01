import { useState } from 'react';
import { Modal, Button, Form, Alert, Spinner } from 'react-bootstrap';
import { shipmentApi } from '../services/api';
import type { CreateShipmentRequest } from '../types/shipment';

interface CreateShipmentModalProps {
  show: boolean;
  onHide: () => void;
  onSuccess: () => void;
  defaultCustomerId?: string;
}

const CreateShipmentModal = ({ show, onHide, onSuccess, defaultCustomerId = '' }: CreateShipmentModalProps) => {
  const [formData, setFormData] = useState<CreateShipmentRequest>({
    ship_name: '',
    ship_address: '',
    customer_id: defaultCustomerId,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccessMessage(null);

    try {
      const response = await shipmentApi.createShipment(formData);

      if (response.success) {
        setSuccessMessage(`Shipment created successfully! ID: ${response.shipment_id}`);
        // Reset form
        setFormData({
          ship_name: '',
          ship_address: '',
          customer_id: defaultCustomerId,
        });
        // Notify parent and close after short delay
        setTimeout(() => {
          onSuccess();
          setSuccessMessage(null);
        }, 1500);
      } else {
        setError('Failed to create shipment');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create shipment');
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setFormData({
      ship_name: '',
      ship_address: '',
      customer_id: defaultCustomerId,
    });
    setError(null);
    setSuccessMessage(null);
    onHide();
  };

  return (
    <Modal show={show} onHide={handleClose} size="lg">
      <Modal.Header closeButton>
        <Modal.Title>Create New Shipment</Modal.Title>
      </Modal.Header>

      <Form onSubmit={handleSubmit}>
        <Modal.Body>
          {error && (
            <Alert variant="danger" dismissible onClose={() => setError(null)}>
              {error}
            </Alert>
          )}

          {successMessage && (
            <Alert variant="success">
              {successMessage}
            </Alert>
          )}

          <Form.Group className="mb-3">
            <Form.Label>Shipment Name <span className="text-danger">*</span></Form.Label>
            <Form.Control
              type="text"
              name="ship_name"
              value={formData.ship_name}
              onChange={handleChange}
              placeholder="e.g., Electronics Package"
              required
              disabled={loading}
            />
            <Form.Text className="text-muted">
              Descriptive name for the shipment
            </Form.Text>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Delivery Address <span className="text-danger">*</span></Form.Label>
            <Form.Control
              as="textarea"
              rows={3}
              name="ship_address"
              value={formData.ship_address}
              onChange={handleChange}
              placeholder="e.g., 123 Main St, New York, NY 10001"
              required
              disabled={loading}
            />
            <Form.Text className="text-muted">
              Complete delivery address
            </Form.Text>
          </Form.Group>

          <Form.Group className="mb-3">
            <Form.Label>Customer ID <span className="text-danger">*</span></Form.Label>
            <Form.Control
              type="text"
              name="customer_id"
              value={formData.customer_id}
              onChange={handleChange}
              placeholder="e.g., CUST001"
              required
              disabled={loading}
            />
            <Form.Text className="text-muted">
              Customer identifier
            </Form.Text>
          </Form.Group>
        </Modal.Body>

        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose} disabled={loading}>
            Cancel
          </Button>
          <Button variant="primary" type="submit" disabled={loading}>
            {loading ? (
              <>
                <Spinner
                  as="span"
                  animation="border"
                  size="sm"
                  role="status"
                  aria-hidden="true"
                  className="me-2"
                />
                Creating...
              </>
            ) : (
              'Create Shipment'
            )}
          </Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
};

export default CreateShipmentModal;
