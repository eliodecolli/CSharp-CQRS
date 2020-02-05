using System;
using RabbitMQ.Client;
using BeeGees.Commands;
using BeeGees.Commands.Responses;
using BeeGees.Queries;
using BeeGees.Queries.Responses;
using System.Collections.Generic;
using System.Diagnostics;
using RabbitMQ.Client.Events;
using BeeGees;
using BeeGees_Messaging;
using System.IO;
using Google.Protobuf;
using System.Threading;
using System.Threading.Tasks;

namespace BeeGees_Client
{
    class Program
    {
        static Dictionary<string, Type> inboundTypes;
        static Dictionary<string, object> inbound;

        static IConnection connection;

        static void SendNewShipment(string shipName)
        {
            var conFac = new ConnectionFactory() { HostName = "localhost" };
            using var connection = conFac.CreateConnection();
            using var model = connection.CreateModel();

            var corrId = Guid.NewGuid().ToString();
            var props = model.CreateBasicProperties();
            props.CorrelationId = corrId;
            props.Persistent = true;

            var shipment = new CreateShipmentCommand()
            {
                CustomerID = "1",
                ShipAddress = "Hogwarts",
                ShipName = shipName
            };

            var bmsg = new BaseMessage()
            {
                Type = (int)MessageType.CreateNewShipment,
                Blob = ByteString.CopyFrom(shipment.ToByteArray())
            };

            model.BasicPublish("writer_exchange", "client_writer", props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentCreatedResponse));
        }

        static void UpdateShipment(string shipId, string location)
        {
            using var model = connection.CreateModel();

            var corrId = Guid.NewGuid().ToString();
            var props = model.CreateBasicProperties();
            props.CorrelationId = corrId;
            props.Persistent = true;

            var shipment = new UpdateShipmentCommand()
            {
                Location = location,
                ShipmentId = shipId,
                Status = "Arrived at " + location
            };

            var bmsg = new BaseMessage()
            {
                Type = (int)MessageType.UpdateShipment,
                Blob = ByteString.CopyFrom(shipment.ToByteArray())
            };

            model.BasicPublish("writer_exchange", "client_writer", props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentUpdatedResponse));
        }

        private static void AskForShipments()
        {
            using var model = connection.CreateModel();

            var corrId = Guid.NewGuid().ToString();
            var props = model.CreateBasicProperties();
            props.CorrelationId = corrId;
            props.Persistent = true;

            var shipment = new GetAllShipmentsQuery()
            {
                CustomerId = "1",
                Sender = "localhost"
            };

            var bmsg = new BaseMessage()
            {
                Type = (int)MessageType.GetAllShipmentsQuery,
                Blob = ByteString.CopyFrom(shipment.ToByteArray())
            };

            model.BasicPublish("reader_exchange", "client_reader", props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(GetAllShipmentsResponse));
        }

        private static void AskForStatus(string shipmentId)
        {
            using var model = connection.CreateModel();

            var corrId = Guid.NewGuid().ToString();
            var props = model.CreateBasicProperties();
            props.CorrelationId = corrId;
            props.Persistent = true;

            var shipment = new GetShipmentStatusQuery()
            {
                Sender = "localhost",
                ShipmentId = shipmentId
            };

            var bmsg = new BaseMessage()
            {
                Type = (int)MessageType.GetShipmentStatusQuery,
                Blob = ByteString.CopyFrom(shipment.ToByteArray())
            };

            model.BasicPublish("reader_exchange", "client_reader", props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(GetShipmentStatusResponse));
        }

        private static void MarkAsDelivered(string shipmentId)
        {
            using var model = connection.CreateModel();

            var corrId = Guid.NewGuid().ToString();
            var props = model.CreateBasicProperties();
            props.CorrelationId = corrId;
            props.Persistent = true;

            var shipment = new MarkShipmentAsDeliveredCommand()
            {
                ShipmentId = shipmentId,
                AdditionalTaxes = 0,
                DeliveredDate = DateTime.Now.ToBinary()
            };

            var bmsg = new BaseMessage()
            {
                Type = (int)MessageType.MarkShipmentAsDelivered,
                Blob = ByteString.CopyFrom(shipment.ToByteArray())
            };

            model.BasicPublish("writer_exchange", "client_writer", props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentDeliveredResponse));
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Press any key to start the client...");
            //Console.Read();
            Thread.Sleep(5000);
            inbound = new Dictionary<string, object>();
            inboundTypes = new Dictionary<string, Type>();

            var stopwatch = new Stopwatch();
            var conFac = new ConnectionFactory() { HostName = "localhost" };
            connection = conFac.CreateConnection();
            using var channelWriter = connection.CreateModel();
            channelWriter.ExchangeDeclare("client", ExchangeType.Direct, true, false, null);
            channelWriter.QueueDeclare("writer_client", true, false, false, null);
            channelWriter.QueueBind("writer_client", "client", "writer_client", null);

            var writerConsumer = new EventingBasicConsumer(channelWriter);
            writerConsumer.Received += writerChannel_Received;
            channelWriter.BasicConsume("writer_client", true, writerConsumer);

            SendNewShipment("Mary Jane");


            // set up reader communications
            using var reader_channel = connection.CreateModel();
            reader_channel.QueueDeclare("reader_client", true, false, false, null);
            reader_channel.ExchangeDeclare("client", "direct", true, false, null);
            reader_channel.QueueBind("reader_client", "client", "reader_client", null);

            var readerConsumer = new EventingBasicConsumer(reader_channel);
            readerConsumer.Received += ReaderConsumer_Received;
            reader_channel.BasicConsume("reader_client", true, readerConsumer);

            Console.WriteLine("waiting");


            start:
            Console.Write(">>> ");
            var cmd = Console.ReadLine();
            if (cmd.StartsWith("update-shipment"))
            {
                var id = cmd.Split(' ')[1];
                var location = cmd.Split(' ')[2];

                UpdateShipment(id, location);
            }
            else if (cmd == "exit")
                return;
            else if(cmd == "get-shipments")
            {
                for (int i = 0; i < 5; i++)
                {
                    /*Task.Run(() =>*/AskForShipments()/*)*/;
                }
            }
            
            goto start;
        }

        private static void ReaderConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return;

            if(inboundTypes.ContainsKey(e.BasicProperties.CorrelationId))
            {
                var type = inboundTypes[e.BasicProperties.CorrelationId];

                if(type == typeof(GetAllShipmentsResponse))
                {
                    // getting our types
                    var msg = GetAllShipmentsResponse.Parser.ParseFrom(e.Body);

                    if (msg.Success)
                    {
                        Console.WriteLine($"Got {msg.Shipments.Count} shipments for request {e.BasicProperties.CorrelationId}");
                        foreach (var s in msg.Shipments)
                        {
                            Console.WriteLine($"Shipment Name: {s.ShipmentName}{Environment.NewLine}Shipment Location: {s.CurrentLocation}");
                        }
                    }
                    else
                        Console.WriteLine("Err.. something wrong happened :/");
                }
                if(type == typeof(GetShipmentStatusResponse))
                {
                    // getting our types
                    var msg = GetShipmentStatusResponse.Parser.ParseFrom(e.Body);

                    if (msg.Success)
                    {
                        Console.WriteLine($"Shipment Name: {msg.ShipmentName}{Environment.NewLine}Shipment Location: {msg.Status}");
                    }
                    else
                        Console.WriteLine("Err.. something wrong happened :/");
                }
            }
        }

        private static int steps = 0;

        private static void writerChannel_Received(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return;

            if(inboundTypes.ContainsKey(e.BasicProperties.CorrelationId))
            {
                //Console.WriteLine($"{e.BasicProperties.CorrelationId} -> Magum P.I. is a great show, the 2018 remake is also pretty good and extremely underrated.");
                //var baseMessage = BaseMessage.Parser.ParseFrom(e.Body);

                var type = inboundTypes[e.BasicProperties.CorrelationId];
                if(type == typeof(ShipmentCreatedResponse))
                {
                    // yaay
                    var message = ShipmentCreatedResponse.Parser.ParseFrom(e.Body);
                    if(message.Success)
                    {
                        Console.WriteLine($"A new shipment with ID {message.ShipmentId} has been created");
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;

                    //UpdateShipment(message.ShipmentId, "City " + steps.ToString());
                }
                else if(type == typeof(ShipmentUpdatedResponse))
                {
                    var message = ShipmentUpdatedResponse.Parser.ParseFrom(e.Body);
                    if(message.Success)
                    {
                        Console.WriteLine($"Shipmet {message.ShipmentName} with ID {message.ShipmentId} has been updated");
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;

                    new Thread(() =>
                    {
                        if (steps < 5)
                        {
                            AskForStatus(message.ShipmentId);
                            UpdateShipment(message.ShipmentId, "City " + steps.ToString());
                            steps++;
                        }
                        else
                        {
                            MarkAsDelivered(message.ShipmentId);
                        }
                    }).Start();
                }
                else if(type == typeof(ShipmentDeliveredResponse))
                {
                    var message = ShipmentDeliveredResponse.Parser.ParseFrom(e.Body);
                    if(message.Success)
                    {
                        Console.WriteLine($"Shipment {message.ShipmentId} has been delivered");
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;
                }
            }
        }
    }
}
