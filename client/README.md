# BeeGees Shipment API - FastAPI Backend

Python FastAPI backend service that bridges frontend applications with the BeeGees CQRS shipment tracking system.

## Features

- RESTful API for shipment operations
- RabbitMQ integration with Write and Read nodes
- Protocol Buffers message serialization (uses proto files from `../src/BeeGees_Messaging`)
- Async request/response correlation handling
- OpenAPI/Swagger documentation

## Setup

1. Create and activate virtual environment:
```bash
python3 -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
```

2. Install dependencies:
```bash
pip install -r requirements.txt
```

3. Copy environment configuration:
```bash
cp .env.example .env
```

4. Generate protobuf Python files:
```bash
python generate_proto.py
```

5. Start the API server:
```bash
uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
```

## Docker Deployment

Build and run using Docker Compose (run from the `client` directory):
```bash
docker-compose up --build
```

This will:
1. Build the Docker image from the parent directory context (to access proto files)
2. Generate Python protobuf files during build
3. Start the FastAPI backend on port 8000
4. Start RabbitMQ on port 62660 (with management UI on 15672)

**Note**: The Docker build context is the parent directory to access `src/BeeGees_Messaging` proto files.

## API Endpoints

- `POST /api/shipments` - Create new shipment
- `GET /api/shipments` - Get all shipments for a customer
- `GET /api/shipments/{shipment_id}` - Get shipment status
- `PUT /api/shipments/{shipment_id}/location` - Update shipment location
- `POST /api/shipments/{shipment_id}/deliver` - Mark shipment as delivered

## Documentation

Once running, visit:
- Swagger UI: http://localhost:8000/docs
- ReDoc: http://localhost:8000/redoc
