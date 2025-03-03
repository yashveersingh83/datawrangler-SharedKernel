using StackExchange.Redis;
using Newtonsoft.Json;
namespace SharedKernel
{
    public interface IRedisCacheService
    {
        Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> GetCacheAsync<T>(string key);
        Task<bool> RemoveCacheAsync(string key);
        Task<bool> ExistsAsync(string key);
    }
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry)
        {
            if (expiry == null)
            {
                expiry = new TimeSpan(1, 0, 0);
            }
            var serializedValue = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key, serializedValue, expiry);
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cachedValue = await _database.StringGetAsync(key);
            if (cachedValue.IsNullOrEmpty)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(cachedValue);
        }

        public async Task<bool> RemoveCacheAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }
    }
}