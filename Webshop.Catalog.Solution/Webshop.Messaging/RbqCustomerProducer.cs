using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Messaging
{
    public class RbqCustomerProducer
    {
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly ConnectionFactory _factory;

        public RbqCustomerProducer(string hostname, string exchangeName, string queueName)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _factory = new ConnectionFactory
            {
                HostName = hostname
            };
        }

        public async Task InitializeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Declare the exchange
            await channel.ExchangeDeclareAsync(
                exchange: _exchangeName,
                type: ExchangeType.Fanout
            );

            // Declare the queue
            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind the queue to the exchange
            await channel.QueueBindAsync(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: string.Empty
            );

            Console.WriteLine($" [x] Exchange '{_exchangeName}' and Queue '{_queueName}' initialized.");
        }

        public async Task SendMessageAsync(string message)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var body = Encoding.UTF8.GetBytes(message);

            // Publish message
            await channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: string.Empty,
                body: body
            );

            Console.WriteLine($" [x] Sent: {message}");
        }
    }
}
