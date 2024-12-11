using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Webshop.Search.Api.Utilities;

namespace Webshop.Search.Api.Tests.Polly
{
    public class PollyPoliciesTests
    {
        [Fact]
        public async Task GetRetryPolicy_RetriesOnTransientHttpError()
        {
            // Arrange
            var retryPolicy = PollyPolicies.GetRetryPolicy(3);
            var callCount = 0;

            // Act
            await retryPolicy.ExecuteAsync(async () =>
            {
                callCount++;
                if (callCount < 3)
                {
                    // Simuler en transient HTTP-fejl
                    throw new HttpRequestException("Transient error");
                }
                // Returner en succesfuld HTTP-svarbesked
                return new HttpResponseMessage(HttpStatusCode.OK);
            });

            // Assert
            Assert.Equal(3, callCount); // Ensures retry logic works
        }

        [Fact]
        public async Task GetCircuitBreakerPolicy_TriggersCircuitBreakerAfterFailures()
        {
            // Arrange
            var circuitBreakerPolicy = PollyPolicies.GetCircuitBreakerPolicy(30);
            int failureThreshold = 2; // Threshold for circuit breaker
            int executionCount = 0;

            // Act: Trigger circuit breaker by causing the specified number of failures
            for (int i = 0; i < failureThreshold; i++)
            {
                await Assert.ThrowsAsync<HttpRequestException>(async () =>
                {
                    await circuitBreakerPolicy.ExecuteAsync(async () =>
                    {
                        executionCount++;
                        throw new HttpRequestException("Transient error");
                    });
                });
            }

            // Assert: Circuit breaker should now be open
            var exception = await Assert.ThrowsAsync<BrokenCircuitException>(async () =>
            {
                await circuitBreakerPolicy.ExecuteAsync(async () =>
                {
                    executionCount++;
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });
            });

            Assert.Equal("The circuit is now open and is not allowing calls.", exception.Message);
            Assert.Equal(failureThreshold, executionCount); // Ensure threshold matches execution count
        }





    }
}
