using StackExchange.Redis;
using System.Threading.Tasks;
using System;
using Webshop.Core.Contracts.Contracts;

namespace Webshop.Search.Api.Services
{
    public class RedisCacheService
    {
        private readonly IRedisConnection _redisConnection;

        public RedisCacheService(IRedisConnection redisConnection)
        {
            _redisConnection = redisConnection;
        }

        public async Task<string> GetValueAsync(string key)
        {
            var db = _redisConnection.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            var db = _redisConnection.GetDatabase();
            await db.StringSetAsync(key, value, expiry);
        }
    }
}
