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
            var validConnectionString = "localhost";
            var mockLogger = new Mock<ILogger<RedisConnection>>();

            // Act
            var redisConnection = new RedisConnection(validConnectionString, mockLogger.Object);

            // Assert
            Assert.NotNull(redisConnection);
            mockLogger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successfully connected to Redis.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }


        [Fact]
        public void RedisConnection_InvalidConnectionString_ThrowsRedisConnectionException()
        {
            // Arrange
            var invalidConnectionString = "invalid_connection_string";
            var mockLogger = new Mock<ILogger<RedisConnection>>();

            // Act & Assert
            Assert.Throws<RedisConnectionException>(() =>
            {
                new RedisConnection(invalidConnectionString, mockLogger.Object);
            });
        }

    }
}