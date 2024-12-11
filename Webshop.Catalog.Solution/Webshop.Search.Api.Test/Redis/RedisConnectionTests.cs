using Moq;
using StackExchange.Redis;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Webshop.RedisService;

namespace Webshop.Search.Api.Tests.Redis
{

    public class RedisConnectionTests
    {
        private readonly Mock<ILogger<RedisConnection>> _mockLogger;

        public RedisConnectionTests()
        {
            _mockLogger = new Mock<ILogger<RedisConnection>>();
        }

        [Fact]
        public void RedisConnection_ValidConnectionString_ConnectsSuccessfully()
        {
            // Arrange
            var validConnectionString = "localhost:6379";

            // Act
            var redisConnection = new RedisConnection(validConnectionString, _mockLogger.Object);
            var database = redisConnection.GetDatabase();

            // Assert
            Assert.NotNull(database);
            _mockLogger.Verify(logger => logger.LogInformation("Successfully connected to Redis."), Times.Once);
        }

        [Fact]
        public void RedisConnection_InvalidConnectionString_ThrowsException()
        {
            // Arrange
            var invalidConnectionString = "invalid:6379";

            // Act & Assert
            var exception = Assert.Throws<Exception>(() =>
                new RedisConnection(invalidConnectionString, _mockLogger.Object));
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Once);
        }
    }
}