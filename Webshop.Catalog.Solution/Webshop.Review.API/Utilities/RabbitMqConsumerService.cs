using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Webshop.Review.Api.Utilities
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IChannel _channel; // Opdatering: IChannel i stedet for IModel

        public RabbitMqConsumerService()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"

            };
            Console.WriteLine("RabbitMqConsumerService constructor called. Service is being initialized.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync(); // Opdatering: Bruger CreateChannelAsync

            string queueName = "ReviewQueue";

            // Declare the queue
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,   // Køen gemmes ved genstart af RabbitMQ
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine($"[*] Waiting for messages in queue '{queueName}'...");

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Process the message here
                Console.WriteLine($"[x] Received: {message}");

                // Simulate async message processing
                await Task.Delay(100); // Kun til demonstration
            };

            // Start consuming messages from the queue
            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true, // Automatisk kvittering
                consumer: consumer
            );

            // Keep the consumer running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            base.Dispose();
        }
    }
}
