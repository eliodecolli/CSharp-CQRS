# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY BeeGees.sln ./
COPY BeeGees_Messaging/BeeGees_Messaging.csproj ./BeeGees_Messaging/
COPY BeeGees_ReadNode/BeeGees_ReadNode.csproj ./BeeGees_ReadNode/

# Restore dependencies
RUN dotnet restore BeeGees_ReadNode/BeeGees_ReadNode.csproj

# Copy all source files
COPY BeeGees_Messaging/ ./BeeGees_Messaging/
COPY BeeGees_ReadNode/ ./BeeGees_ReadNode/

# Build the application
WORKDIR /src/BeeGees_ReadNode
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create directory for logs
RUN mkdir -p /app/logs

# Set environment variables with default values
ENV DB_HOST=postgres \
    DB_PORT=5432 \
    DB_NAME=BeeGees_Reader \
    DB_USER=postgres \
    DB_PASSWORD=postgres \
    RABBITMQ_HOST=rabbitmq \
    RABBITMQ_PORT=5672

ENTRYPOINT ["dotnet", "BeeGees_ReadNode.dll"]
