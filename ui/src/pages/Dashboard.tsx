import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { Container, Row, Col, Card, Table, Button, Alert, Spinner, Form, Badge } from 'react-bootstrap';
import { shipmentApi } from '../services/api';
import type { ShipmentResponse } from '../types/shipment';
import CreateShipmentModal from '../components/CreateShipmentModal';

const Dashboard = () => {
  const [shipments, setShipments] = useState<ShipmentResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [customerId, setCustomerId] = useState('CUST001'); // Default customer ID
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  useEffect(() => {
    loadShipments();
  }, [customerId, refreshTrigger]);

  const loadShipments = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await shipmentApi.getAllShipments(customerId);

      if (response.success) {
        setShipments(response.shipments);
      } else {
        setError('Failed to load shipments');
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load shipments');
    } finally {
      setLoading(false);
    }
  };

  const handleShipmentCreated = () => {
    setShowCreateModal(false);
    setRefreshTrigger(prev => prev + 1); // Trigger refresh
  };

  const formatDateTime = (dateString: string) => {
    try {
      return new Date(dateString).toLocaleString();
    } catch {
      return dateString;
    }
  };

  return (
    <Container fluid className="py-4">
      <Row className="mb-4">
        <Col>
          <div className="d-flex justify-content-between align-items-center">
            <h1>Shipment Dashboard</h1>
            <Button variant="primary" onClick={() => setShowCreateModal(true)}>
              Create New Shipment
            </Button>
          </div>
        </Col>
      </Row>

      <Row className="mb-4">
        <Col md={6}>
          <Form.Group>
            <Form.Label>Customer ID</Form.Label>
            <Form.Control
              type="text"
              value={customerId}
              onChange={(e) => setCustomerId(e.target.value)}
              placeholder="Enter customer ID"
            />
            <Form.Text className="text-muted">
              Enter a customer ID to filter shipments
            </Form.Text>
          </Form.Group>
        </Col>
        <Col md={6} className="d-flex align-items-end">
          <Button variant="secondary" onClick={loadShipments}>
            Refresh
          </Button>
        </Col>
      </Row>

      {error && (
        <Alert variant="danger" dismissible onClose={() => setError(null)}>
          {error}
        </Alert>
      )}

      {loading ? (
        <div className="text-center py-5">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </div>
      ) : (
        <Card>
          <Card.Header>
            <h5 className="mb-0">Shipments for Customer: {customerId}</h5>
          </Card.Header>
          <Card.Body>
            {shipments.length === 0 ? (
              <Alert variant="info">
                No shipments found for this customer. Create your first shipment to get started!
              </Alert>
            ) : (
              <Table striped bordered hover responsive>
                <thead>
                  <tr>
                    <th>Shipment ID</th>
                    <th>Name</th>
                    <th>Current Location</th>
                    <th>Status</th>
                    <th>Last Update</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {shipments.map((shipment) => (
                    <tr key={shipment.shipment_id}>
                      <td>
                        <code>{shipment.shipment_id}</code>
                      </td>
                      <td>{shipment.shipment_name}</td>
                      <td>{shipment.current_location}</td>
                      <td>
                        <Badge bg={shipment.status === 'Delivered' ? 'success' : 'info'}>
                          {shipment.status}
                        </Badge>
                      </td>
                      <td>{formatDateTime(shipment.last_status_update)}</td>
                      <td>
                        <Link to={`/shipment/${shipment.shipment_id}`}>
                          <Button variant="primary" size="sm">
                            View Details
                          </Button>
                        </Link>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </Table>
            )}
          </Card.Body>
        </Card>
      )}

      <CreateShipmentModal
        show={showCreateModal}
        onHide={() => setShowCreateModal(false)}
        onSuccess={handleShipmentCreated}
        defaultCustomerId={customerId}
      />
    </Container>
  );
};

export default Dashboard;
