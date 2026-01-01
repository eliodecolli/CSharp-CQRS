import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import {
  Container,
  Row,
  Col,
  Card,
  Button,
  Alert,
  Spinner,
  Form,
  Badge,
  Modal,
} from 'react-bootstrap';
import { shipmentApi } from '../services/api';
import type { GetShipmentStatusResponse, UpdateShipmentLocationRequest, MarkShipmentDeliveredRequest } from '../types/shipment';

const ShipmentDetail = () => {
  const { id } = useParams<{ id: string }>();

  const [shipment, setShipment] = useState<GetShipmentStatusResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  // Update location form
  const [showUpdateForm, setShowUpdateForm] = useState(false);
  const [updateData, setUpdateData] = useState<UpdateShipmentLocationRequest>({
    location: '',
    status: '',
  });
  const [updating, setUpdating] = useState(false);

  // Deliver modal
  const [showDeliverModal, setShowDeliverModal] = useState(false);

  // Helper function to get current datetime in local format for datetime-local input
  const getCurrentDateTimeLocal = () => {
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  };

  const [deliverData, setDeliverData] = useState<MarkShipmentDeliveredRequest>({
    delivered_date: getCurrentDateTimeLocal(),
    additional_taxes: 0,
  });
  const [delivering, setDelivering] = useState(false);

  useEffect(() => {
    if (id) {
      loadShipmentStatus();
    }
  }, [id]);

  const loadShipmentStatus = async () => {
    if (!id) return;

    try {
      setLoading(true);
      setError(null);
      const response = await shipmentApi.getShipmentStatus(id);

      if (response.success) {
        setShipment(response);
      } else {
        setError('Shipment not found');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load shipment');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateLocation = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;

    try {
      setUpdating(true);
      setError(null);
      const response = await shipmentApi.updateShipmentLocation(id, updateData);

      if (response.success) {
        setSuccessMessage('Shipment location updated successfully!');
        setShowUpdateForm(false);

        // Immediately update local state to reflect new status
        if (shipment) {
          setShipment({
            ...shipment,
            status: updateData.status,
            last_updated: new Date().toISOString(),
          });
        }

        setUpdateData({ location: '', status: '' });

        // Also refresh from server to ensure consistency
        setTimeout(async () => {
          await loadShipmentStatus();
        }, 500);

        setTimeout(() => setSuccessMessage(null), 3000);
      } else {
        setError('Failed to update shipment');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update shipment');
    } finally {
      setUpdating(false);
    }
  };

  const handleMarkDelivered = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!id) return;

    try {
      setDelivering(true);
      setError(null);
      const response = await shipmentApi.markShipmentDelivered(id, deliverData);

      if (response.success) {
        setSuccessMessage('Shipment marked as delivered successfully!');
        setShowDeliverModal(false);
        setDeliverData({ delivered_date: getCurrentDateTimeLocal(), additional_taxes: 0 });

        // Immediately update local state to reflect delivered status
        if (shipment) {
          setShipment({
            ...shipment,
            status: 'Delivered',
            last_updated: new Date().toISOString(),
          });
        }

        // Also refresh from server to ensure consistency
        setTimeout(async () => {
          await loadShipmentStatus();
        }, 500);

        setTimeout(() => setSuccessMessage(null), 3000);
      } else {
        setError('Failed to mark shipment as delivered');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to mark shipment as delivered');
    } finally {
      setDelivering(false);
    }
  };

  const formatDateTime = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleString();
    } catch {
      return dateString;
    }
  };

  if (loading) {
    return (
      <Container fluid className="py-5 text-center">
        <Spinner animation="border" role="status">
          <span className="visually-hidden">Loading...</span>
        </Spinner>
      </Container>
    );
  }

  if (error && !shipment) {
    return (
      <Container fluid className="py-5">
        <Alert variant="danger">
          {error}
        </Alert>
        <Link to="/">
          <Button variant="primary">Back to Dashboard</Button>
        </Link>
      </Container>
    );
  }

  return (
    <Container fluid className="py-4">
      <Row className="mb-4">
        <Col>
          <Link to="/" className="text-decoration-none">
            <Button variant="outline-secondary" size="sm">
              &larr; Back to Dashboard
            </Button>
          </Link>
        </Col>
      </Row>

      {successMessage && (
        <Alert variant="success" dismissible onClose={() => setSuccessMessage(null)}>
          {successMessage}
        </Alert>
      )}

      {error && (
        <Alert variant="danger" dismissible onClose={() => setError(null)}>
          {error}
        </Alert>
      )}

      {shipment && (
        <>
          <Row>
            <Col lg={8}>
              <Card className="mb-4">
                <Card.Header>
                  <h4 className="mb-0">
                    Shipment Details{' '}
                    <Badge bg="primary" className="ms-2">
                      {id}
                    </Badge>
                  </h4>
                </Card.Header>
                <Card.Body>
                  <Row className="mb-3">
                    <Col md={4}>
                      <strong>Shipment Name:</strong>
                    </Col>
                    <Col md={8}>{shipment.shipment_name}</Col>
                  </Row>
                  <Row className="mb-3">
                    <Col md={4}>
                      <strong>Current Status:</strong>
                    </Col>
                    <Col md={8}>
                      <Badge bg={shipment.status === 'Delivered' ? 'success' : 'info'}>
                        {shipment.status}
                      </Badge>
                    </Col>
                  </Row>
                  <Row className="mb-3">
                    <Col md={4}>
                      <strong>Last Updated:</strong>
                    </Col>
                    <Col md={8}>{formatDateTime(shipment.last_updated)}</Col>
                  </Row>
                </Card.Body>
              </Card>

              <Card className="mb-4">
                <Card.Header>
                  <h5 className="mb-0">Update Shipment Location</h5>
                </Card.Header>
                <Card.Body>
                  {shipment.status === 'Delivered' ? (
                    <Alert variant="info" className="mb-0">
                      <strong>Updates Disabled</strong>
                      <br />
                      This shipment has been delivered and can no longer be updated.
                    </Alert>
                  ) : !showUpdateForm ? (
                    <Button variant="primary" onClick={() => setShowUpdateForm(true)}>
                      Update Location & Status
                    </Button>
                  ) : (
                    <Form onSubmit={handleUpdateLocation}>
                      <Form.Group className="mb-3">
                        <Form.Label>New Location <span className="text-danger">*</span></Form.Label>
                        <Form.Control
                          type="text"
                          value={updateData.location}
                          onChange={(e) =>
                            setUpdateData({ ...updateData, location: e.target.value })
                          }
                          placeholder="e.g., Distribution Center - Chicago"
                          required
                          disabled={updating}
                        />
                      </Form.Group>

                      <Form.Group className="mb-3">
                        <Form.Label>Status Message <span className="text-danger">*</span></Form.Label>
                        <Form.Control
                          as="textarea"
                          rows={2}
                          value={updateData.status}
                          onChange={(e) =>
                            setUpdateData({ ...updateData, status: e.target.value })
                          }
                          placeholder="e.g., Arrived at Distribution Center"
                          required
                          disabled={updating}
                        />
                      </Form.Group>

                      <div className="d-flex gap-2">
                        <Button variant="primary" type="submit" disabled={updating}>
                          {updating ? (
                            <>
                              <Spinner
                                as="span"
                                animation="border"
                                size="sm"
                                role="status"
                                className="me-2"
                              />
                              Updating...
                            </>
                          ) : (
                            'Update Shipment'
                          )}
                        </Button>
                        <Button
                          variant="secondary"
                          onClick={() => {
                            setShowUpdateForm(false);
                            setUpdateData({ location: '', status: '' });
                          }}
                          disabled={updating}
                        >
                          Cancel
                        </Button>
                      </div>
                    </Form>
                  )}
                </Card.Body>
              </Card>
            </Col>

            <Col lg={4}>
              <Card className="mb-4 border-success">
                <Card.Header className="bg-success text-white">
                  <h5 className="mb-0">Delivery Actions</h5>
                </Card.Header>
                <Card.Body>
                  {shipment.status === 'Delivered' ? (
                    <Alert variant="success" className="mb-0">
                      <strong>Shipment Delivered!</strong>
                      <br />
                      This shipment has already been marked as delivered.
                    </Alert>
                  ) : (
                    <>
                      <p className="text-muted mb-3">
                        Mark this shipment as delivered when it reaches its destination.
                      </p>
                      <Button
                        variant="success"
                        className="w-100"
                        onClick={() => setShowDeliverModal(true)}
                      >
                        Mark as Delivered
                      </Button>
                    </>
                  )}
                </Card.Body>
              </Card>

              <Card>
                <Card.Header>
                  <h5 className="mb-0">Quick Info</h5>
                </Card.Header>
                <Card.Body>
                  <p className="mb-2">
                    <strong>Shipment ID:</strong>
                    <br />
                    <code>{id}</code>
                  </p>
                  <hr />
                  <p className="text-muted mb-0">
                    Track and update your shipment status in real-time using the forms on this page.
                  </p>
                </Card.Body>
              </Card>
            </Col>
          </Row>
        </>
      )}

      {/* Mark as Delivered Modal */}
      <Modal
        show={showDeliverModal}
        onHide={() => {
          setShowDeliverModal(false);
          setDeliverData({ delivered_date: getCurrentDateTimeLocal(), additional_taxes: 0 });
        }}
      >
        <Modal.Header closeButton>
          <Modal.Title>Mark Shipment as Delivered</Modal.Title>
        </Modal.Header>

        <Form onSubmit={handleMarkDelivered}>
          <Modal.Body>
            <Form.Group className="mb-3">
              <Form.Label>Delivery Date & Time</Form.Label>
              <Form.Control
                type="datetime-local"
                value={deliverData.delivered_date || ''}
                onChange={(e) =>
                  setDeliverData({ ...deliverData, delivered_date: e.target.value })
                }
                disabled={delivering}
              />
              <Form.Text className="text-muted">
                Leave empty to use current date/time
              </Form.Text>
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Additional Taxes (in cents)</Form.Label>
              <Form.Control
                type="number"
                min="0"
                value={deliverData.additional_taxes}
                onChange={(e) =>
                  setDeliverData({
                    ...deliverData,
                    additional_taxes: parseInt(e.target.value) || 0,
                  })
                }
                disabled={delivering}
              />
              <Form.Text className="text-muted">
                Enter any additional charges in cents (e.g., 500 = $5.00)
              </Form.Text>
            </Form.Group>
          </Modal.Body>

          <Modal.Footer>
            <Button
              variant="secondary"
              onClick={() => {
                setShowDeliverModal(false);
                setDeliverData({ delivered_date: getCurrentDateTimeLocal(), additional_taxes: 0 });
              }}
              disabled={delivering}
            >
              Cancel
            </Button>
            <Button variant="success" type="submit" disabled={delivering}>
              {delivering ? (
                <>
                  <Spinner
                    as="span"
                    animation="border"
                    size="sm"
                    role="status"
                    className="me-2"
                  />
                  Processing...
                </>
              ) : (
                'Confirm Delivery'
              )}
            </Button>
          </Modal.Footer>
        </Form>
      </Modal>
    </Container>
  );
};

export default ShipmentDetail;
