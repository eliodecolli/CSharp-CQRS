import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { Container, Navbar } from 'react-bootstrap';
import Dashboard from './pages/Dashboard';
import ShipmentDetail from './pages/ShipmentDetail';
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

function App() {
  return (
    <Router>
      <div className="min-vh-100 d-flex flex-column">
        {/* Navigation Bar */}
        <Navbar bg="dark" variant="dark" expand="lg" className="mb-0">
          <Container fluid>
            <Navbar.Brand href="/">
              <strong>BeeGees</strong> Shipment Tracking
            </Navbar.Brand>
            <Navbar.Toggle />
            <Navbar.Collapse className="justify-content-end">
              <Navbar.Text className="text-light">
                CQRS Shipment Management System
              </Navbar.Text>
            </Navbar.Collapse>
          </Container>
        </Navbar>

        {/* Main Content */}
        <main className="flex-grow-1 bg-light">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/shipment/:id" element={<ShipmentDetail />} />
          </Routes>
        </main>

        {/* Footer */}
        <footer className="bg-dark text-light py-3 mt-auto">
          <Container fluid>
            <div className="text-center">
              <small>
                BeeGees Shipment Tracking System - Built with React, Vite, Bootstrap 5, and CQRS Architecture
              </small>
            </div>
          </Container>
        </footer>
      </div>
    </Router>
  );
}

export default App;
