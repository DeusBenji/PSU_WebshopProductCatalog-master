using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Webshop.Core.Contracts.Contracts;

namespace Webshop.RedisService
{
    public class RedisConnection : IRedisConnection, IDisposable
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<RedisConnection> _logger;

        public RedisConnection(string connectionString, ILogger<RedisConnection> logger)
        {
            _logger = logger;

            try
            {
                _connection = ConnectionMultiplexer.Connect(connectionString);
                _logger.LogInformation("Successfully connected to Redis.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to connect to Redis: {ex.Message}");
                throw;
            }
        }

        public IDatabase GetDatabase()
        {
            _logger.LogInformation("Retrieving Redis database instance.");
            return _connection.GetDatabase();
        }

        public void Dispose()
        {
            _logger.LogInformation("Disposing Redis connection.");
            _connection?.Dispose();
        }
    }
}
