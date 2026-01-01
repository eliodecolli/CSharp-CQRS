FROM python:3.11-slim

WORKDIR /app

# Install system dependencies
RUN apt-get update && apt-get install -y \
    gcc \
    && rm -rf /var/lib/apt/lists/*

# Copy requirements first for better caching
COPY client/requirements.txt .

# Install Python dependencies
RUN pip install --no-cache-dir -r requirements.txt

# Copy proto files from main project (needed for generation)
COPY src/BeeGees_Messaging /proto_source/BeeGees_Messaging

# Copy proto generation script
COPY client/generate_proto.py .

# Generate protobuf Python files
RUN python generate_proto.py

# Copy application code
COPY client/app/ ./app/

# Expose port
EXPOSE 8000

# Run the application
CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
