using BeeGees.Queries;
using BeeGees.Queries.Responses;
using BeeGees;
using BeeGees_Messaging;
using BeeGees_ReadNode.Facade;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.IO;
using Google.Protobuf;
using BeeGees.Events;

namespace BeeGees_ReadNode
{
    public class ReaderEntryPoint
    {
        private readonly ConnectionFactory connectionFactory;
        private readonly IFacade facade;

        private  IConnection connection;

        private  IModel clientChannel;
        private  IModel eventsChannel;

        public ReaderEntryPoint()
        {
            connectionFactory = new ConnectionFactory();
            facade = new ReaderFacade();

            facade.InitializeFastAccess(3);
        }

        public void WaitForConnections()
        {
            connection = connectionFactory.CreateConnection();
            clientChannel = connection.CreateModel();
            eventsChannel = connection.CreateModel();


            // create a new reader exchange
            clientChannel.ExchangeDeclare("reader_exchange", ExchangeType.Direct, true, false);
            var queue = clientChannel.QueueDeclare("client_reader");
            clientChannel.QueueBind("client_reader", "reader_exchange", "client_reader");
            var consumerClient = new EventingBasicConsumer(clientChannel);
            consumerClient.Received += Consumer_Received;

            eventsChannel.ExchangeDeclare("events", ExchangeType.Direct, true, false);
            eventsChannel.QueueDeclare("writer_sourcing");
            eventsChannel.QueueBind("writer_sourcing", "events", "writer_sourcing");
            var consumerEvent = new EventingBasicConsumer(eventsChannel);
            consumerEvent.Received += ConsumerEvent_Received;

            clientChannel.BasicConsume("client_reader", true, consumerClient);
            eventsChannel.BasicConsume("writer_sourcing", true, consumerEvent);

            Log.Debug(" [x] Reader is now waiting for incoming calls :)");
        }

        private void ConsumerEvent_Received(object sender, BasicDeliverEventArgs e)
        {
            //Log.Info($" [x] Received a new event {e.DeliveryTag} from writer sourcing");

            var message = BaseMessage.Parser.ParseFrom(e.Body);
            IMessage response = null;

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

            SubmitToEventsAggregate("reader_confirmation", response.ToByteArray(), e.BasicProperties.CorrelationId);
        }

        private void SubmitToClient(string queue, byte[] blob, string id)
        {
            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("client", ExchangeType.Direct, true, false);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.CorrelationId = id;

                channel.BasicPublish("client", queue, props, blob);
            }
        }

        private void SubmitToEventsAggregate(string queue, byte[] blob, string correlationId)
        {
            using (var channel = connection.CreateModel())
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.CorrelationId = correlationId;

                channel.BasicPublish("events", queue, props, blob);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            //Log.Info($" [x] Received message {e.DeliveryTag} from client");

            var message = BaseMessage.Parser.ParseFrom(e.Body);
            IMessage response = null;
            
            switch((MessageType)message.Type)
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
                    }break;
            }

            SubmitToClient("reader_client", response.ToByteArray(), e.BasicProperties.CorrelationId);
        }
    }
}
