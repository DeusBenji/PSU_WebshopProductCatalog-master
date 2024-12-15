using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using Webshop.Messaging.Contracts;

namespace Webshop.Messaging
{
    public class RbqCustomerProducer : IRbqCustomerProducer
    {
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly ConnectionFactory _factory;

        public RbqCustomerProducer(string hostname, string exchangeName, string queueName)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _factory = new ConnectionFactory { HostName = hostname };
        }

        // Initialiser exchange og queue
        public async Task InitializeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: _exchangeName,
                type: ExchangeType.Fanout
            );

            await channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            await channel.QueueBindAsync(
                queue: _queueName,
                exchange: _exchangeName,
                routingKey: string.Empty
            );
        }
        public async Task SendMessageAsync(string message)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: _exchangeName,
                routingKey: string.Empty,
                body: body
            );
        }

    }
}
