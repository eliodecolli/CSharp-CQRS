"""
RabbitMQ service for communicating with CQRS nodes
"""
import asyncio
import uuid
import logging
from typing import Dict, Any, Optional, Callable
from datetime import datetime

import aio_pika
from aio_pika import Message, ExchangeType, DeliveryMode
from aio_pika.abc import AbstractIncomingMessage

from app.config import settings

logger = logging.getLogger(__name__)


class RabbitMQService:
    """Service for managing RabbitMQ connections and message handling"""

    def __init__(self):
        self.connection: Optional[aio_pika.Connection] = None
        self.writer_channel: Optional[aio_pika.Channel] = None
        self.reader_channel: Optional[aio_pika.Channel] = None
        self.response_channel: Optional[aio_pika.Channel] = None

        # Correlation tracking
        self.pending_requests: Dict[str, asyncio.Future] = {}
        self.request_types: Dict[str, type] = {}

    async def connect(self):
        """Establish connection to RabbitMQ and set up exchanges/queues"""
        logger.info(
            f"Connecting to RabbitMQ at {settings.rabbitmq_host}:{settings.rabbitmq_port}"
        )

        # Create connection
        self.connection = await aio_pika.connect_robust(
            host=settings.rabbitmq_host,
            port=settings.rabbitmq_port,
            login=settings.rabbitmq_user,
            password=settings.rabbitmq_password,
        )

        # Create channels
        self.writer_channel = await self.connection.channel()
        self.reader_channel = await self.connection.channel()
        self.response_channel = await self.connection.channel()

        # Declare exchanges
        await self._setup_exchanges()

        # Set up response queues and consumers
        await self._setup_response_queues()

        logger.info("✓ RabbitMQ connection established")

    async def _setup_exchanges(self):
        """Declare all required exchanges"""
        # Writer exchange
        await self.writer_channel.declare_exchange(
            "writer_exchange", ExchangeType.DIRECT, durable=True
        )

        # Reader exchange
        await self.reader_channel.declare_exchange(
            "reader_exchange", ExchangeType.DIRECT, durable=True
        )

        # Client exchange for responses
        await self.response_channel.declare_exchange(
            "client", ExchangeType.DIRECT, durable=True
        )

        logger.info("✓ Exchanges declared")

    async def _setup_response_queues(self):
        """Set up queues for receiving responses from CQRS nodes"""
        client_exchange = await self.response_channel.declare_exchange(
            "client", ExchangeType.DIRECT, durable=True
        )

        # Queue for writer responses
        writer_queue = await self.response_channel.declare_queue(
            "writer_client", durable=True
        )
        await writer_queue.bind(client_exchange, routing_key="writer_client")

        # Queue for reader responses
        reader_queue = await self.response_channel.declare_queue(
            "reader_client", durable=True
        )
        await reader_queue.bind(client_exchange, routing_key="reader_client")

        # Start consuming
        await writer_queue.consume(self._handle_writer_response)
        await reader_queue.consume(self._handle_reader_response)

        logger.info("✓ Response queues configured and consuming")

    async def _handle_writer_response(self, message: AbstractIncomingMessage):
        """Handle responses from Write Node"""
        async with message.process():
            correlation_id = message.correlation_id

            if not correlation_id:
                logger.warning("Received message without correlation ID")
                return

            if correlation_id in self.pending_requests:
                logger.debug(f"Received writer response for {correlation_id}")
                future = self.pending_requests[correlation_id]
                future.set_result(bytes(message.body))
                del self.pending_requests[correlation_id]
                if correlation_id in self.request_types:
                    del self.request_types[correlation_id]
            else:
                logger.warning(f"Received response for unknown correlation ID: {correlation_id}")

    async def _handle_reader_response(self, message: AbstractIncomingMessage):
        """Handle responses from Read Node"""
        async with message.process():
            correlation_id = message.correlation_id

            if not correlation_id:
                logger.warning("Received message without correlation ID")
                return

            if correlation_id in self.pending_requests:
                logger.debug(f"Received reader response for {correlation_id}")
                future = self.pending_requests[correlation_id]
                future.set_result(bytes(message.body))
                del self.pending_requests[correlation_id]
                if correlation_id in self.request_types:
                    del self.request_types[correlation_id]
            else:
                logger.warning(f"Received response for unknown correlation ID: {correlation_id}")

    async def send_command(
        self, message_body: bytes, routing_key: str = "client_writer"
    ) -> bytes:
        """
        Send a command to the Write Node and wait for response

        Args:
            message_body: Serialized BaseMessage protobuf
            routing_key: Routing key for the message

        Returns:
            Response bytes from Write Node
        """
        correlation_id = str(uuid.uuid4())

        # Create future for tracking response
        future = asyncio.Future()
        self.pending_requests[correlation_id] = future

        # Create message
        message = Message(
            body=message_body,
            correlation_id=correlation_id,
            delivery_mode=DeliveryMode.NOT_PERSISTENT,
        )

        # Send to writer exchange
        exchange = await self.writer_channel.declare_exchange(
            "writer_exchange", ExchangeType.DIRECT, durable=True
        )

        await exchange.publish(message, routing_key=routing_key)

        logger.debug(f"Sent command with correlation ID: {correlation_id}")

        # Wait for response with timeout
        try:
            response = await asyncio.wait_for(future, timeout=settings.request_timeout)
            return response
        except asyncio.TimeoutError:
            # Clean up on timeout
            if correlation_id in self.pending_requests:
                del self.pending_requests[correlation_id]
            if correlation_id in self.request_types:
                del self.request_types[correlation_id]
            raise TimeoutError(
                f"Request timed out after {settings.request_timeout} seconds"
            )

    async def send_query(
        self, message_body: bytes, routing_key: str = "client_reader"
    ) -> bytes:
        """
        Send a query to the Read Node and wait for response

        Args:
            message_body: Serialized BaseMessage protobuf
            routing_key: Routing key for the message

        Returns:
            Response bytes from Read Node
        """
        correlation_id = str(uuid.uuid4())

        # Create future for tracking response
        future = asyncio.Future()
        self.pending_requests[correlation_id] = future

        # Create message
        message = Message(
            body=message_body,
            correlation_id=correlation_id,
            delivery_mode=DeliveryMode.NOT_PERSISTENT,
        )

        # Send to reader exchange
        exchange = await self.reader_channel.declare_exchange(
            "reader_exchange", ExchangeType.DIRECT, durable=True
        )

        await exchange.publish(message, routing_key=routing_key)

        logger.debug(f"Sent query with correlation ID: {correlation_id}")

        # Wait for response with timeout'Q
        try:
            response = await asyncio.wait_for(future, timeout=settings.request_timeout)
            return response
        except asyncio.TimeoutError:
            # Clean up on timeout
            if correlation_id in self.pending_requests:
                del self.pending_requests[correlation_id]
            if correlation_id in self.request_types:
                del self.request_types[correlation_id]
            raise TimeoutError(
                f"Request timed out after {settings.request_timeout} seconds"
            )

    async def close(self):
        """Close RabbitMQ connection"""
        if self.connection:
            await self.connection.close()
            logger.info("RabbitMQ connection closed")


# Global instance
rabbitmq_service = RabbitMQService()