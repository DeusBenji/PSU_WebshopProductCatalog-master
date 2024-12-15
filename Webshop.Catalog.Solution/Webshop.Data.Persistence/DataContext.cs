using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Data;

namespace Webshop.Data.Persistence
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataContext> _logger;
        private readonly string _sqlConnectionString;
        private readonly string _redisConnectionString;
        private IConnectionMultiplexer _redisConnection;

        public DataContext(IConfiguration configuration, ILogger<DataContext> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Hent SQL-forbindelsesstrengen
            _sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTIONSTRING")
                                   ?? _configuration.GetConnectionString("DefaultConnection");
            _logger.LogInformation($"Using SQL connection string: {_sqlConnectionString}");

            // Hent Redis-forbindelsesstrengen
            _redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTIONSTRING")
                                     ?? _configuration.GetConnectionString("RedisConnection");
            _logger.LogInformation($"Using Redis connection string: {_redisConnectionString}");

            InitializeRedis();
        }

        /// <summary>
        /// Opret en ny SQL-forbindelse (uændret for at undgå kodeændringer andre steder).
        /// </summary>
        public IDbConnection CreateConnection()
        {
            _logger.LogInformation("Creating SQL database connection.");
            return new System.Data.SqlClient.SqlConnection(_sqlConnectionString);
        }

        /// <summary>
        /// Hent Redis-databasen.
        /// </summary>
        public IDatabase GetRedisDatabase()
        {
            _logger.LogInformation("Retrieving Redis database instance.");
            if (_redisConnection == null || !_redisConnection.IsConnected)
            {
                InitializeRedis();
            }
            return _redisConnection.GetDatabase();
        }

        private void InitializeRedis()
        {
            try
            {
                _redisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
                _logger.LogInformation("Successfully connected to Redis.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to connect to Redis: {ex.Message}");
                throw;
            }
        }
    }
}