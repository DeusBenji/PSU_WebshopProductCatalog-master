using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
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
                HostName = hostname,
                UserName = "guest",
                Password = "guest"
            };
        }

        public async Task InitializeAsync()
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: _exchangeName, type: ExchangeType.Fanout);
            await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queue: _queueName, exchange: _exchangeName, routingKey: string.Empty);
        }

        public async Task SendMessageAsync<T>(T message)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var jsonMessage = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: string.Empty, body: body);

            Console.WriteLine($"[x] Sent: {jsonMessage}");
        }
    }
}
