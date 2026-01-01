# BeeGees Shipment Tracking - React Frontend

A modern React frontend for the BeeGees CQRS shipment tracking system, built with Vite, TypeScript, and Bootstrap 5.

## Features

- **Dashboard**: View all shipments for a customer with real-time status
- **Create Shipment**: Easy-to-use modal for creating new shipments
- **Shipment Details**: View and update individual shipment information
- **Update Location**: Track shipment progress as it moves through delivery
- **Mark as Delivered**: Complete the delivery process with delivery date and additional charges
- **Responsive Design**: Mobile-friendly interface using Bootstrap 5

## Tech Stack

- **React 19** with TypeScript
- **Vite 5** for fast development and building
- **Bootstrap 5** for responsive UI components
- **React Router** for navigation
- **Axios** for API communication
- **React-Bootstrap** for React-friendly Bootstrap components

## Prerequisites

- Node.js 18+ (tested with Node.js 18.19.1)
- pnpm (recommended) or npm
- BeeGees FastAPI backend running on port 8000

## Installation

1. Install dependencies using pnpm:
```bash
pnpm install
```

Or with npm:
```bash
npm install
```

2. Configure environment variables:
```bash
cp .env.example .env
```

Edit `.env` to set your API backend URL:
```
VITE_API_BASE_URL=http://localhost:8000
```

## Development

Start the development server with pnpm:
```bash
pnpm run dev
```

Or with npm:
```bash
npm run dev
```

The application will be available at `http://localhost:5173`

## Building for Production

Build the application:
```bash
pnpm run build
```

Preview the production build:
```bash
pnpm run preview
```

## Project Structure

```
ui/
├── src/
│   ├── components/          # Reusable React components
│   │   └── CreateShipmentModal.tsx
│   ├── pages/              # Page components
│   │   ├── Dashboard.tsx   # Main dashboard/shipment list
│   │   └── ShipmentDetail.tsx  # Individual shipment view
│   ├── services/           # API service layer
│   │   └── api.ts          # Axios client and API methods
│   ├── types/              # TypeScript type definitions
│   │   └── shipment.ts     # Shipment-related types
│   ├── App.tsx             # Main app component with routing
│   ├── App.css             # Custom styles
│   └── main.tsx            # Application entry point
├── .env                    # Environment configuration
├── package.json
└── vite.config.ts
```

## API Integration

The frontend communicates with the FastAPI backend through the following endpoints:

- `POST /api/shipments` - Create new shipment
- `GET /api/shipments?customer_id={id}` - Get all shipments
- `GET /api/shipments/{id}` - Get shipment status
- `PUT /api/shipments/{id}/location` - Update shipment location
- `POST /api/shipments/{id}/deliver` - Mark shipment as delivered

## Usage Guide

### Creating a Shipment

1. Click "Create New Shipment" on the dashboard
2. Fill in the shipment details:
   - Shipment Name (e.g., "Electronics Package")
   - Delivery Address
   - Customer ID
3. Click "Create Shipment"

### Viewing Shipments

- Enter a Customer ID in the dashboard filter
- Click "Refresh" to load shipments for that customer
- All shipments are displayed in a table with current status

### Updating Shipment Location

1. Click "View Details" on any shipment
2. Click "Update Location & Status"
3. Enter new location and status message
4. Click "Update Shipment"

### Marking as Delivered

1. Navigate to shipment details page
2. Click "Mark as Delivered" in the right sidebar
3. Optionally set delivery date and additional taxes
4. Click "Confirm Delivery"

## Configuration

### API Base URL

Set the API backend URL in `.env`:
```
VITE_API_BASE_URL=http://localhost:8000
```

For production, update this to your production API URL.

## Troubleshooting

### API Connection Issues

- Ensure the FastAPI backend is running on the configured port
- Check CORS settings on the backend allow requests from the frontend origin
- Verify the `VITE_API_BASE_URL` in `.env` is correct

### Node.js Version Compatibility

This project uses Vite 5.x which is compatible with Node.js 18+. If you're using Node.js 18, the project will work fine. For Node.js 20+, you can upgrade Vite to version 7 if desired.

### Port Already in Use

If port 5173 is in use:
```bash
pnpm run dev -- --port 3000
```

## Quick Start (All Services)

To run the complete system:

1. **Start RabbitMQ** (in Docker):
```bash
docker run -d --name rabbitmq -p 62660:5672 rabbitmq:3-management
```

2. **Start Write Node** (from project root):
```bash
cd src/BeeGees_WriteNode
dotnet run
```

3. **Start Read Node** (from project root):
```bash
cd src/BeeGees_ReadNode
dotnet run
```

4. **Start FastAPI Backend** (from client folder):
```bash
cd client
python generate_proto.py
uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
```

5. **Start React Frontend** (from ui folder):
```bash
cd ui
pnpm run dev
```

Then open http://localhost:5173 in your browser!

## Contributing

When adding new features:
1. Add TypeScript types in `src/types/`
2. Create API methods in `src/services/api.ts`
3. Build components in `src/components/` or pages in `src/pages/`
4. Update routing in `App.tsx` if needed

## License

Part of the BeeGees CQRS Shipment Tracking System.
