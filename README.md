![DOTNET CI](https://github.com/eliodecolli/CSharp-CQRS/actions/workflows/dotnet.yml/badge.svg)

# BeeGees - Shipment Tracking System

A practical implementation of the CQRS (Command Query Responsibility Segregation) pattern built with .NET 8.0, demonstrating event-driven architecture and eventual consistency through a shipment tracking domain model.

## Overview

This project implements a distributed shipment tracking system where write and read operations are handled by separate, specialized nodes. The architecture follows CQRS principles with event sourcing to maintain data consistency across independent databases.

**Core Capabilities:**
- Create and manage shipments with delivery tracking
- Update shipment status and location in real-time
- Query shipment information with intelligent caching
- Maintain eventual consistency through event-driven synchronization

## Architecture

The system consists of three primary components that communicate via RabbitMQ message queues:

### 1. **Write Node** (`BeeGees_WriteNode`)
Responsible for all state-changing operations (commands). The write node:
- Processes commands: `CreateShipment`, `UpdateShipment`, `MarkShipmentAsDelivered`
- Maintains the authoritative data store (PostgreSQL)
- Generates domain events for each state change
- Uses the Facade pattern to encapsulate business logic

### 2. **Read Node** (`BeeGees_ReadNode`)
Handles all read operations (queries) with performance optimizations. The read node:
- Processes queries: `GetAllShipments`, `GetShipmentStatus`
- Maintains a denormalized read model (PostgreSQL)
- Implements a custom in-memory caching system with smart invalidation
- Consumes events from the write node to stay synchronized
- Provides fast query responses through cached data structures

### 3. **Client** (`BeeGees_Client`)
A console application demonstrating integration with both nodes:
- Sends commands to the write node
- Queries data from the read node
- Manages correlation IDs for request/response matching
- Serves as a reference implementation for external integrations

### 4. **API Backend** (`client/`)
FastAPI-based REST API providing HTTP access to the system:
- Exposes RESTful endpoints for shipment management
- Translates HTTP requests to Protocol Buffer commands/queries
- Communicates with Write and Read nodes via RabbitMQ
- Provides OpenAPI documentation at `/docs`
- Built with Python 3.11 and FastAPI

### 5. **Web UI** (`ui/`)
Modern React-based web interface:
- Dashboard for viewing all shipments
- Real-time shipment status tracking
- Delivery management with status indicators
- Color-coded status badges (green for delivered)
- Responsive design with React Bootstrap
- Built with Vite, React 18, and TypeScript

### 6. **Messaging** (`BeeGees_Messaging`)
Shared message contracts using Protocol Buffers:
- Command definitions (CreateShipmentCommand, UpdateShipmentCommand, etc.)
- Query definitions (GetAllShipmentsQuery, GetShipmentStatusQuery, etc.)
- Event definitions (ShipmentCreatedEvent, ShipmentUpdatedEvent, etc.)
- Response messages for all operations
- Base message envelope for type routing

### 7. **Test Projects**
- `BeeGees_WriteNode.Tests` - Unit tests for write node operations
- `BeeGees_ReadNode.Tests` - Unit tests for read node operations and cache behavior

## Technical Stack

**Backend:**
- **.NET 8.0** - Modern C# with async/await patterns
- **Python 3.11** - FastAPI backend for REST API
- **RabbitMQ** - Message broker for inter-component communication
- **Protocol Buffers** - Efficient binary serialization (enables polyglot implementations)
- **PostgreSQL** - Separate databases for write and read nodes
- **Entity Framework Core** - Data access and migrations

**Frontend:**
- **React 18** - Modern UI library with hooks
- **TypeScript** - Type-safe JavaScript
- **Vite** - Fast build tool and dev server
- **React Bootstrap** - UI component library
- **React Router** - Client-side routing

**DevOps:**
- **Docker** - Containerization for all services
- **Docker Compose** - Multi-container orchestration
- **Nginx** - Static file serving for production UI

## Message Flow & Synchronization

The system implements eventual consistency through an event-driven synchronization mechanism:

```
Client → Command → Write Node → Event → Read Node
                 ↓
            Response → Client
```

**Detailed Flow:**

1. Client publishes a command to the `writer_exchange` with a unique correlation ID
2. Write node receives the command via the `client_writer` queue
3. Write node validates and processes the command, updating its database
4. Write node generates a corresponding event (e.g., `ShipmentCreatedEvent`) and sends response to client
5. Write node publishes the event to the `events` exchange
6. Read node consumes the event from the `writer_sourcing` queue
7. Read node applies changes to its database and invalidates affected cache entries
8. Client receives the response via the `writer_client` queue using the correlation ID

**Why this approach?**
- Provides fast command responses without waiting for read node synchronization
- Allows the read node to be optimized independently without affecting write operations
- Maintains eventual consistency between write and read databases

**Trade-off:** Read queries immediately after a write may return slightly stale data until the read node processes the event. This is an acceptable trade-off for improved performance.

## RabbitMQ Topology

### Exchanges
- `writer_exchange` (Direct) - Routes commands to write node
- `reader_exchange` (Direct) - Routes queries to read node
- `events` (Direct) - Routes events between nodes
- `client` (Direct) - Routes responses back to clients

### Queues & Routing
| Queue | Bound To | Routing Key | Purpose |
|-------|----------|-------------|---------|
| `client_writer` | writer_exchange | client_writer | Receives commands from clients |
| `client_reader` | reader_exchange | client_reader | Receives queries from clients |
| `writer_sourcing` | events | writer_sourcing | Delivers events to read node |
| `reader_confirmation` | events | reader_confirmation | Delivers confirmations to write node |
| `writer_client` | client | writer_client | Delivers command responses to clients |
| `reader_client` | client | reader_client | Delivers query responses to clients |

### Message Types
All messages are wrapped in a `BaseMessage` envelope containing:
- `Type` (int) - Message type identifier
- `Blob` (bytes) - Serialized Protocol Buffer payload

**Supported Message Types:**
1. `CreateNewShipment` - Create a new shipment
2. `UpdateShipment` - Update shipment location and status
3. `MarkShipmentAsDelivered` - Mark shipment as delivered
4. `ShipmentCreatedEvent` - Event: shipment was created
5. `ShipmentUpdatedEvent` - Event: shipment was updated
6. `ShipmentDeliveredEvent` - Event: shipment was delivered
7. `GetAllShipmentsQuery` - Query: get all shipments for a customer
8. `GetShipmentStatusQuery` - Query: get specific shipment status

## Intelligent In-Memory Caching

The read node implements a custom caching system to minimize database hits for repeated queries.

### Cache Navigation Strategy

Cached items are stored using a hierarchical path structure called **Cached Query String (CQS)**:

```
{SENDER}/{QUERY_TYPE}/{PARAM_NAME}={PARAM_VALUE}/{PARAM_NAME}={PARAM_VALUE}/...
```

**Example:**
```
backend-api/GetAllShipmentsQuery/CustomerId=12345
```

**Wildcard Support:**
Using `*` as the sender allows cache hits regardless of the requesting client:
```
*/GetAllShipmentsQuery/CustomerId=12345
```

### Cache Invalidation

When the read node receives an event that modifies data, it automatically invalidates affected cache entries:
- `ShipmentUpdatedEvent` → Invalidates cached queries containing that shipment
- `ShipmentDeliveredEvent` → Invalidates cached queries for that customer and shipment
- Uses reflection to match event properties against cached query parameters

This prevents dirty reads while maximizing cache hit rates for unchanged data.

### Parameter Matching

Additional filtering is supported through reflection:
```
*/GetAllShipmentsQuery/CustomerId=12345/Status=InTransit
```

**Requirement:** Parameter names must match property names on the query result entities. For example, if querying `Shipment` objects, valid parameters include `ShipmentId`, `Status`, `CurrentLocation`, etc.

## Running the Application

### Prerequisites
- .NET 8.0 SDK (for local development)
- Docker & Docker Compose (for containerized deployment)

### Option 1: Docker Compose (Recommended)

Run the entire stack with a single command:

```bash
cd docker
docker-compose up --build
```

This starts:
- PostgreSQL (port 61660)
- RabbitMQ (port 62660)
- Write Node (containerized)
- Read Node (containerized)
- FastAPI Backend (port 8000)
- React UI (port 3000)

The nodes will automatically run migrations and start processing messages.

**Access the application:**
- **Web UI**: http://localhost:3000
- **API**: http://localhost:8000
- **API Docs**: http://localhost:8000/docs

**Run the .NET client to interact with the system:**
```bash
cd src/BeeGees_Client
dotnet run
```

**View logs:**
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f write-node
docker-compose logs -f read-node
docker-compose logs -f api
docker-compose logs -f ui
```

**Stop the stack:**
```bash
docker-compose down
```

### Option 2: Local Development

Run nodes locally for development and debugging:

1. **Start infrastructure only:**
   ```bash
   cd docker
   docker-compose up -d postgres rabbitmq
   ```

2. **Run database migrations:**
   ```bash
   cd src/BeeGees_WriteNode
   dotnet ef database update

   cd ../BeeGees_ReadNode
   dotnet ef database update
   ```

3. **Start the write node:**
   ```bash
   cd src/BeeGees_WriteNode
   dotnet run
   ```

4. **Start the read node (in a new terminal):**
   ```bash
   cd src/BeeGees_ReadNode
   dotnet run
   ```

5. **Run the client (in a new terminal):**
   ```bash
   cd src/BeeGees_Client
   dotnet run
   ```

### Running Tests

```bash
cd src
dotnet test
```

## Project Roadmap

**Completed:**
- ✅ Upgraded from .NET 3.1 to .NET 8.0 with modern language features
- ✅ Reworked RabbitMQ routing with proper exchange topology
- ✅ Migrated from SQL Server to PostgreSQL
- ✅ Added CI pipeline with automated testing
- ✅ Containerized with Docker
- ✅ Unit tests for write and read nodes
- ✅ Complete UI/API implementation (React + FastAPI)
- ✅ Real-time shipment status tracking with delivery management
- ✅ Responsive dashboard with status indicators

## Design Decisions

### Why Protocol Buffers?
The original vision was to implement each node in a different language (e.g., write node in C#, read node in Go). Protocol Buffers provide language-agnostic serialization, making this architecture truly polyglot-ready.

### Why Separate Databases?
CQRS emphasizes separation of concerns. The write database is normalized for transactional integrity, while the read database can be denormalized for query performance. This allows independent scaling and optimization strategies.

### Why Eventual Consistency?
The system favors performance and availability over immediate consistency. Commands return quickly after the write node processes them, while the read node catches up asynchronously. This design allows for better scalability and responsiveness.

## Contributing

Contributions are welcome! Areas of particular interest:
- Enhanced error handling and resilience patterns
- Performance optimizations for cache invalidation
- Additional query types and projections
- Frontend implementations (React, Angular, Vue)
- Alternative language implementations of nodes

## License

This project is provided as-is for educational and demonstration purposes.

## Acknowledgments

Built to explore CQRS, event sourcing, and distributed systems architecture in a practical domain context.
