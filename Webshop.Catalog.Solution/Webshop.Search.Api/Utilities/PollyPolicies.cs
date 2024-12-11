using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Webshop.Search.Api.Utilities
{
    public static class PollyPolicies
    {
        /// <summary>
        /// Retry policy with exponential backoff.
        /// </summary>
        /// <param name="retryCount">Number of retries before failing.</param>
        /// <returns>A Polly retry policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryCount, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Circuit breaker policy to block calls after failures.
        /// </summary>
        /// <param name="durationInSeconds">Duration in seconds to block calls after threshold is reached.</param>
        /// <returns>A Polly circuit breaker policy.</returns>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int durationInSeconds)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(durationInSeconds));
        }
    }
}
