using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Webshop.Messaging.Routing
{
    public class Router
    {
        private readonly ConnectionFactory _factory;

        public Router(string hostname)
        {
            _factory = new ConnectionFactory
            {
                HostName = hostname
            };
        }

        // Konfigurer exchange og kø
        public async Task ConfigureExchangeAndQueueAsync(string exchangeName, string queueName)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Deklarér exchange
            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Fanout
            );

            // Deklarér kø
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind kø til exchange
            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: string.Empty
            );
        }

        // Metode til at oprette forbindelse
        public async Task<IConnection> CreateConnectionAsync()
        {
            return await _factory.CreateConnectionAsync();
        }
    }
}
