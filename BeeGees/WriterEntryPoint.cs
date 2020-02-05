using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Google.Protobuf;
using BeeGees;
using BeeGees.Commands;
using BeeGees_Messaging;
using System.IO;
using BeeGees_WriteNode.Facade;
using BeeGees.Commands.Responses;
using System.Collections.Generic;
using BeeGees.Events;
using System;

namespace BeeGees_WriteNode
{
    public class WriterEntryPoint
    {
        private ConnectionFactory connectionFactory;
        //private IMessageHandler createShipmentHandler, updateShipmentHandler;

        private IConnection connection;
        private IModel channelClient;
        private IModel channelEvents;

        private readonly IWriterFacade facade;

        private readonly Dictionary<string, IMessage> hodlingOn;

        public WriterEntryPoint(IWriterFacade facade)
        {
            connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            this.facade = facade;
            hodlingOn = new Dictionary<string, IMessage>();
        }

        public void WaitForWork()
        {
            connection = connectionFactory.CreateConnection();
            channelClient = connection.CreateModel();
            channelEvents = connection.CreateModel();

            channelClient.ExchangeDeclare("writer_exchange", ExchangeType.Direct, true, false, null);
            var queue = channelClient.QueueDeclare("client_writer", true, false, false, null);

            channelClient.QueueBind("client_writer", "writer_exchange", "client_writer");

            var consumer = new EventingBasicConsumer(channelClient);
            consumer.Received += Consumer_Received;

            channelClient.BasicConsume("client_writer", true, consumer);


            // now do the events
            channelEvents.ExchangeDeclare("events", ExchangeType.Direct, true, false);
            channelEvents.QueueDeclare("reader_confirmation", true, false, false, null);
            channelEvents.QueueBind("reader_confirmation", "events", "reader_confirmation", null);

            var eventsConsumer = new EventingBasicConsumer(channelEvents);
            eventsConsumer.Received += EventsConsumer_Received;
            channelEvents.BasicConsume("reader_confirmation", true, eventsConsumer);

            Log.Debug("Waiting for calls :)");
        }

        private void EventsConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return;

            try
            {
                var msg = ReaderConfirmation.Parser.ParseFrom(e.Body);
                if(msg.Success)
                {
                    if (hodlingOn.ContainsKey(e.BasicProperties.CorrelationId))
                    {
                        SubmitToClient("writer_client", hodlingOn[e.BasicProperties.CorrelationId].ToByteArray(), e.BasicProperties.CorrelationId);
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
                Log.Error($" [x] Something fishy happened while reading the event :/");
            }
        }

        private void SubmitToClient(string queue, byte[] blob, string id)
        {
            using (var channel = connection.CreateModel())
            {
                /*channel.ExchangeDeclare("client", ExchangeType.Direct, true, false);
                channel.QueueDeclare(queue, true, false, false, null);
                channel.QueueBind(queue, "client", "");*/

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
            //Log.Info($"Received message {e.DeliveryTag.ToString()} :D");

            // here we read what we need and redirect the blob to our router
            var message = BaseMessage.Parser.ParseFrom(e.Body);
            IMessage response = null;
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

            if(response != null)
            {
                // nope wait for a confirmation from the reader
                //SubmitToClient("writer_client", response.ToByteArray(), e.BasicProperties.CorrelationId);
                hodlingOn.Add(e.BasicProperties.CorrelationId, response);

                // if it's now a fact then update the events
                if (success)
                {
                    var fact = facade.GenerateEvent(response);

                    SubmitToEventsAggregate("writer_sourcing", fact.ToByteArray(), e.BasicProperties.CorrelationId);    // this should be done in the same transaction as the command execution tho =\
                    /*
                           A better approach would be to have a different database where to save events and act as an event aggregate. Use Log Tailing to get the latest updates, or publish/subscribe just for notifying the other services 
                    */
                }
            }
        }
    }
}
