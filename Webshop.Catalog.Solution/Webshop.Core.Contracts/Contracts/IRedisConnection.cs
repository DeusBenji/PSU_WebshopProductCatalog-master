using StackExchange.Redis;

namespace Webshop.Core.Contracts.Contracts
{
    /// <summary>
    /// Interface for managing Redis connections.
    /// </summary>
    public interface IRedisConnection : IDisposable
    {
        /// <summary>
        /// Retrieves the Redis database instance.
        /// </summary>
        /// <returns>An instance of <see cref="IDatabase"/>.</returns>
        IDatabase GetDatabase();
    }
}
