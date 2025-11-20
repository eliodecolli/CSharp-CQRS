using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees;
using BeeGees_Messaging;
using BeeGees_ReadNode.Facade;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Google.Protobuf;
using BeeGees.Events;

namespace BeeGees_ReadNode
{
    public class ReaderEntryPoint
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly IFacade facade;

        private IConnection? connection;

        private IChannel? clientChannel;
        private IChannel? eventsChannel;

        public ReaderEntryPoint()
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost", Port = 62660 };
            facade = new ReaderFacade();

            facade.InitializeFastAccess(3);
        }

        public async Task WaitForConnectionsAsync()
        {
            connection = await connectionFactory.CreateConnectionAsync();
            clientChannel = await connection.CreateChannelAsync();
            eventsChannel = await connection.CreateChannelAsync();

            // create a new reader exchange
            await clientChannel.ExchangeDeclareAsync("reader_exchange", ExchangeType.Direct, true, false);
            await clientChannel.QueueDeclareAsync("client_reader", false, false, false, null);
            await clientChannel.QueueBindAsync("client_reader", "reader_exchange", "client_reader");
            var consumerClient = new AsyncEventingBasicConsumer(clientChannel);
            consumerClient.ReceivedAsync += Consumer_ReceivedAsync;

            await eventsChannel.ExchangeDeclareAsync("events", ExchangeType.Direct, true, false);
            await eventsChannel.QueueDeclareAsync("writer_sourcing", false, false, false, null);
            await eventsChannel.QueueBindAsync("writer_sourcing", "events", "writer_sourcing");
            var consumerEvent = new AsyncEventingBasicConsumer(eventsChannel);
            consumerEvent.ReceivedAsync += ConsumerEvent_ReceivedAsync;

            await clientChannel.BasicConsumeAsync("client_reader", true, consumerClient);
            await eventsChannel.BasicConsumeAsync("writer_sourcing", true, consumerEvent);

            Log.Debug(" [x] Reader is now waiting for incoming calls :)");
        }

        private async Task ConsumerEvent_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            Log.Info(" [x] Received consumer event");
            var message = BaseMessage.Parser.ParseFrom(e.Body.ToArray());
            IMessage? response = null;

            switch ((MessageType)message.Type)
            {
                case MessageType.ShipmentCreatedEvent:
                    {
                        var packetMessage = ShipmentCreatedEvent.Parser.ParseFrom(message.Blob);
                        response = facade.GenerateHandler(MessageType.ShipmentCreatedEvent).HandleMessage(packetMessage);
                    }
                    break;
                case MessageType.ShipmentUpdatedEvent:
                    {
                        var packetMessage = ShipmentUpdatedEvent.Parser.ParseFrom(message.Blob);
                        response = facade.GenerateHandler(MessageType.ShipmentUpdatedEvent).HandleMessage(packetMessage);
                    }
                    break;
                case MessageType.ShipmentDeliveredEvent:
                    {
                        var packetMessage = ShipmentDeliveredEvent.Parser.ParseFrom(message.Blob);
                        response = facade.GenerateHandler(MessageType.ShipmentDeliveredEvent).HandleMessage(packetMessage);
                    }
                    break;
            }

            if (response != null)
            {
                await SubmitToEventsAggregateAsync("reader_confirmation", response.ToByteArray(), e.BasicProperties.CorrelationId!);
            }
            else
            {
                Log.Error($" [x] Cannot process message {Enum.GetName((MessageType)message.Type)}");
            }
        }

        private async Task SubmitToClientAsync(string queue, byte[] blob, string id)
        {
            using var channel = await connection!.CreateChannelAsync();

            await channel.ExchangeDeclareAsync("client", ExchangeType.Direct, true, false);

            var props = new BasicProperties
            {
                Persistent = false,
                CorrelationId = id
            };

            await channel.BasicPublishAsync("client", queue, false, props, blob);
        }

        private async Task SubmitToEventsAggregateAsync(string queue, byte[] blob, string correlationId)
        {
            using var channel = await connection!.CreateChannelAsync();

            var props = new BasicProperties
            {
                Persistent = false,
                CorrelationId = correlationId
            };

            await channel.BasicPublishAsync("events", queue, false, props, blob);
        }

        private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            var message = BaseMessage.Parser.ParseFrom(e.Body.ToArray());
            IMessage? response = null;

            switch ((MessageType)message.Type)
            {
                case MessageType.GetAllShipmentsQuery:
                    {
                        var packetMessage = GetAllShipmentsQuery.Parser.ParseFrom(message.Blob);
                        response = (GetAllShipmentsResponse)facade.GenerateHandler(MessageType.GetAllShipmentsQuery).HandleMessage(packetMessage);
                    }
                    break;
                case MessageType.GetShipmentStatusQuery:
                    {
                        var packetMessage = GetShipmentStatusQuery.Parser.ParseFrom(message.Blob);
                        response = (GetShipmentStatusResponse)facade.GenerateHandler(MessageType.GetShipmentStatusQuery).HandleMessage(packetMessage);
                    }
                    break;
            }

            if (response != null)
            {
                await SubmitToClientAsync("reader_client", response.ToByteArray(), e.BasicProperties.CorrelationId!);
            }
        }
    }
}
