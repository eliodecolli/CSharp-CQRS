using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Google.Protobuf;
using BeeGees;
using BeeGees.Commands;
using BeeGees_Messaging;
using BeeGees_WriteNode.Facade;
using BeeGees.Commands.Responses;
using BeeGees.Events;

namespace BeeGees_WriteNode
{
    public class WriterEntryPoint
    {
        private readonly ConnectionFactory connectionFactory;

        private IConnection? connection;
        private IChannel? channelClient;
        private IChannel? channelEvents;

        private readonly IWriterFacade facade;

        private readonly Dictionary<string, IMessage> hodlingOn;

        public WriterEntryPoint(IWriterFacade facade)
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost", Port = 62660 };
            this.facade = facade;
            hodlingOn = new Dictionary<string, IMessage>();
        }

        public async Task WaitForWorkAsync()
        {
            connection = await connectionFactory.CreateConnectionAsync();
            channelClient = await connection.CreateChannelAsync();
            channelEvents = await connection.CreateChannelAsync();

            await channelClient.ExchangeDeclareAsync("writer_exchange", ExchangeType.Direct, true, false, null);
            await channelClient.QueueDeclareAsync("client_writer", true, false, false, null);
            await channelClient.QueueBindAsync("client_writer", "writer_exchange", "client_writer");

            var consumer = new AsyncEventingBasicConsumer(channelClient);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await channelClient.BasicConsumeAsync("client_writer", true, consumer);

            // now do the events
            await channelEvents.ExchangeDeclareAsync("events", ExchangeType.Direct, true, false);
            await channelEvents.QueueDeclareAsync("reader_confirmation", true, false, false, null);
            await channelEvents.QueueBindAsync("reader_confirmation", "events", "reader_confirmation", null);

            var eventsConsumer = new AsyncEventingBasicConsumer(channelEvents);
            eventsConsumer.ReceivedAsync += EventsConsumer_ReceivedAsync;
            await channelEvents.BasicConsumeAsync("reader_confirmation", true, eventsConsumer);

            Log.Debug("Waiting for calls :)");
        }

        private async Task EventsConsumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return;

            try
            {
                var msg = ReaderConfirmation.Parser.ParseFrom(e.Body.ToArray());
                if (msg.Success)
                {
                    if (hodlingOn.ContainsKey(e.BasicProperties.CorrelationId))
                    {
                        await SubmitToClientAsync("writer_client", hodlingOn[e.BasicProperties.CorrelationId].ToByteArray(), e.BasicProperties.CorrelationId);
                        hodlingOn.Remove(e.BasicProperties.CorrelationId);
                    }
                    else
                        Log.Error($" [x] Cannot find correlation id {e.BasicProperties.CorrelationId}");
                }
                else
                {
                    Log.Error($" [x] Yikes! Apparently the reader node couldn't update itself :/ CID: {e.BasicProperties.CorrelationId}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($" [x] Something fishy happened while reading the event :/ {ex.Message}");
            }
        }

        private async Task SubmitToClientAsync(string queue, byte[] blob, string id)
        {
            using var channel = await connection!.CreateChannelAsync();

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
            bool success = false;

            switch ((MessageType)message.Type)
            {
                case MessageType.CreateNewShipment:
                    {
                        var blobedMessage = CreateShipmentCommand.Parser.ParseFrom(message.Blob);
                        Log.Info(" [x] Received CreateNewShipmentCommand");

                        response = facade.CreateHandler(HandlerType.CreateShipment).HandleMessage(blobedMessage);
                        success = ((ShipmentCreatedResponse)response).Success;
                    }
                    break;

                case MessageType.MarkShipmentAsDelivered:
                    {
                        var blobedMessage = MarkShipmentAsDeliveredCommand.Parser.ParseFrom(message.Blob);
                        Log.Info(" [x] Received MarkShipmentAsDeliveredCommand");

                        response = facade.CreateHandler(HandlerType.UpdateShimpent).HandleMessage(blobedMessage);
                        success = ((ShipmentDeliveredResponse)response).Success;
                    }
                    break;
                case MessageType.UpdateShipment:
                    {
                        var blobedMessage = UpdateShipmentCommand.Parser.ParseFrom(message.Blob);
                        Log.Info(" [x] Received UpdateShipmentCommand");

                        response = facade.CreateHandler(HandlerType.UpdateShimpent).HandleMessage(blobedMessage);
                        success = ((ShipmentUpdatedResponse)response).Success;
                    }
                    break;
            }

            if (response != null)
            {
                hodlingOn.Add(e.BasicProperties.CorrelationId!, response);

                if (success)
                {
                    var fact = facade.GenerateEvent(response);

                    await SubmitToEventsAggregateAsync("writer_sourcing", fact.ToByteArray(), e.BasicProperties.CorrelationId!);
                }
            }
        }
    }
}
