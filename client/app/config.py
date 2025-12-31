from pydantic_settings import BaseSettings


class Settings(BaseSettings):
    """Application settings"""

    # RabbitMQ Configuration
    rabbitmq_host: str = "localhost"
    rabbitmq_port: int = 62660
    rabbitmq_user: str = "guest"
    rabbitmq_password: str = "guest"

    # API Configuration
    api_title: str = "BeeGees Shipment API"
    api_version: str = "1.0.0"
    api_description: str = "FastAPI backend for BeeGees CQRS shipment tracking system"

    # Request timeout in seconds
    request_timeout: int = 30

    class Config:
        env_file = ".env"
        case_sensitive = False


settings = Settings()
