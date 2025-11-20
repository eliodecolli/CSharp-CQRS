using RabbitMQ.Client;
using BeeGees.Commands;
using BeeGees.Commands.Responses;
using BeeGees.Queries;
using BeeGees.Queries.Responses;
using RabbitMQ.Client.Events;
using BeeGees;
using BeeGees_Messaging;
using Google.Protobuf;

namespace BeeGees_Client
{
    class Program
    {
        static Dictionary<string, Type> inboundTypes = new();
        static Dictionary<string, object> inbound = new();
        static Dictionary<string, TaskCompletionSource> pendingResponses = new();

        static IConnection? connection;

        static async Task SendNewShipmentAsync(string shipName)
        {
            var conFac = new ConnectionFactory() { HostName = "localhost", Port = 62660 };
            await using var conn = await conFac.CreateConnectionAsync();
            await using var channel = await conn.CreateChannelAsync();

            var corrId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = corrId,
                Persistent = false
            };

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

            await channel.BasicPublishAsync("writer_exchange", "client_writer", false, props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentCreatedResponse));

            var tcs = new TaskCompletionSource();
            pendingResponses.Add(corrId, tcs);
            await tcs.Task;
        }

        static async Task UpdateShipmentAsync(string shipId, string location)
        {
            await using var channel = await connection!.CreateChannelAsync();

            var corrId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = corrId,
                Persistent = false
            };

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

            await channel.BasicPublishAsync("writer_exchange", "client_writer", false, props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentUpdatedResponse));

            var tcs = new TaskCompletionSource();
            pendingResponses.Add(corrId, tcs);
            await tcs.Task;
        }

        private static async Task AskForShipmentsAsync()
        {
            await using var channel = await connection!.CreateChannelAsync();

            var corrId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = corrId,
                Persistent = false
            };

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

            await channel.BasicPublishAsync("reader_exchange", "client_reader", false, props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(GetAllShipmentsResponse));

            var tcs = new TaskCompletionSource();
            pendingResponses.Add(corrId, tcs);
            await tcs.Task;
        }

        private static async Task AskForStatusAsync(string shipmentId)
        {
            await using var channel = await connection!.CreateChannelAsync();

            var corrId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = corrId,
                Persistent = false
            };

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

            await channel.BasicPublishAsync("reader_exchange", "client_reader", false, props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(GetShipmentStatusResponse));
        }

        private static async Task MarkAsDeliveredAsync(string shipmentId)
        {
            await using var channel = await connection!.CreateChannelAsync();

            var corrId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = corrId,
                Persistent = false
            };

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

            await channel.BasicPublishAsync("writer_exchange", "client_writer", false, props, bmsg.ToByteArray());
            inboundTypes.Add(corrId, typeof(ShipmentDeliveredResponse));
        }

        static async Task Main(string[] args)
        {
            await Task.Delay(5000);

            var conFac = new ConnectionFactory() { HostName = "localhost", Port = 62660 };
            connection = await conFac.CreateConnectionAsync();

            await using var channelWriter = await connection.CreateChannelAsync();
            await channelWriter.ExchangeDeclareAsync("client", ExchangeType.Direct, true, false, null);
            await channelWriter.QueueDeclareAsync("writer_client", true, false, false, null);
            await channelWriter.QueueBindAsync("writer_client", "client", "writer_client", null);

            var writerConsumer = new AsyncEventingBasicConsumer(channelWriter);
            writerConsumer.ReceivedAsync += WriterChannel_ReceivedAsync;
            await channelWriter.BasicConsumeAsync("writer_client", true, writerConsumer);

            await SendNewShipmentAsync("Mary Jane");

            // set up reader communications
            await using var reader_channel = await connection.CreateChannelAsync();
            await reader_channel.QueueDeclareAsync("reader_client", true, false, false, null);
            await reader_channel.ExchangeDeclareAsync("client", ExchangeType.Direct, true, false, null);
            await reader_channel.QueueBindAsync("reader_client", "client", "reader_client", null);

            var readerConsumer = new AsyncEventingBasicConsumer(reader_channel);
            readerConsumer.ReceivedAsync += ReaderConsumer_ReceivedAsync;
            await reader_channel.BasicConsumeAsync("reader_client", true, readerConsumer);

            Console.WriteLine();
            Console.WriteLine("=== BeeGees Client Ready ===");
            Console.WriteLine("Commands: get-shipments, update-shipment <id> <location>, exit");
            Console.WriteLine();

            while (true)
            {
                Console.Write(">>> ");
                var cmd = Console.ReadLine();

                if (string.IsNullOrEmpty(cmd))
                    continue;

                if (cmd.StartsWith("update-shipment"))
                {
                    var parts = cmd.Split(' ');
                    if (parts.Length >= 3)
                    {
                        var id = parts[1];
                        var location = parts[2];
                        await UpdateShipmentAsync(id, location);
                    }
                }
                else if (cmd == "exit")
                    return;
                else if (cmd == "get-shipments")
                {
                    await AskForShipmentsAsync();
                }
            }
        }

        private static void CleanUpPendingResponse(string correlationId)
        {
            if (pendingResponses.ContainsKey(correlationId))
            {
                pendingResponses[correlationId].SetResult();
                pendingResponses.Remove(correlationId);
            }
        }

        private static Task ReaderConsumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return Task.CompletedTask;

            if (inboundTypes.ContainsKey(e.BasicProperties.CorrelationId))
            {
                var type = inboundTypes[e.BasicProperties.CorrelationId];

                if (type == typeof(GetAllShipmentsResponse))
                {
                    var msg = GetAllShipmentsResponse.Parser.ParseFrom(e.Body.ToArray());

                    if (msg.Success)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Found {msg.Shipments.Count} shipment(s):");
                        Console.WriteLine();
                        foreach (var s in msg.Shipments)
                        {
                            Console.WriteLine($"  Shipment Name: {s.ShipmentName}");
                            Console.WriteLine($"  Location: {s.CurrentLocation}");
                            Console.WriteLine($"  ID: {s.ShipmentId}");
                            Console.WriteLine();
                        }
                    }
                    else
                        Console.WriteLine("Err.. something wrong happened :/");
                }
                if (type == typeof(GetShipmentStatusResponse))
                {
                    var msg = GetShipmentStatusResponse.Parser.ParseFrom(e.Body.ToArray());

                    if (msg.Success)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"  Shipment Name: {msg.ShipmentName}");
                        Console.WriteLine($"  Location: {msg.Status}");
                        Console.WriteLine();
                    }
                    else
                        Console.WriteLine("Err.. something wrong happened :/");
                }
            }

            CleanUpPendingResponse(e.BasicProperties.CorrelationId);

            return Task.CompletedTask;
        }

        private static async Task WriterChannel_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            if (string.IsNullOrEmpty(e.BasicProperties.CorrelationId))
                return;

            if (inboundTypes.ContainsKey(e.BasicProperties.CorrelationId))
            {
                var type = inboundTypes[e.BasicProperties.CorrelationId];
                if (type == typeof(ShipmentCreatedResponse))
                {
                    var message = ShipmentCreatedResponse.Parser.ParseFrom(e.Body.ToArray());
                    if (message.Success)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"✓ Shipment created with ID: {message.ShipmentId}");
                        Console.WriteLine();
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;
                }
                else if (type == typeof(ShipmentUpdatedResponse))
                {
                    var message = ShipmentUpdatedResponse.Parser.ParseFrom(e.Body.ToArray());
                    if (message.Success)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"✓ Shipment '{message.ShipmentName}' (ID: {message.ShipmentId}) has been updated");
                        Console.WriteLine();
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;
                }
                else if (type == typeof(ShipmentDeliveredResponse))
                {
                    var message = ShipmentDeliveredResponse.Parser.ParseFrom(e.Body.ToArray());
                    if (message.Success)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"✓ Shipment {message.ShipmentId} has been delivered");
                        Console.WriteLine();
                    }
                    inbound[e.BasicProperties.CorrelationId] = message;
                }
            }
            
            CleanUpPendingResponse(e.BasicProperties.CorrelationId);
        }
    }
}
