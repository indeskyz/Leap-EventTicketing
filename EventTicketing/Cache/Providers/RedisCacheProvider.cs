using EventTicketing.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace EventTicketing.Cache.Providers
{
    public class RedisCacheProvider : ICacheProvider
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisCacheProvider> _logger;
        public CacheLevel Level => CacheLevel.Distributed;

        public RedisCacheProvider(
            IDistributedCache cache,
            IConnectionMultiplexer redis,
            ILogger<RedisCacheProvider> logger)
        {
            _cache = cache;
            _redis = redis;
            _logger = logger;
        }

        public async Task<(bool exists, T? value)> TryGetAsync<T>(string key)
        {
            try
            {
                var data = await _cache.GetStringAsync(key);
                if (data == null)
                {
                    _logger.LogDebug("Cache miss for key: {Key}", key);
                    return (false, default);
                }

                _logger.LogDebug("Cache hit for key: {Key}", key);
                return (true, JsonSerializer.Deserialize<T>(data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis read error for key: {Key}", key);
                return (false, default);
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration)
        {
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };

                await _cache.SetStringAsync(
                    key,
                    JsonSerializer.Serialize(value),
                    options);

                _logger.LogDebug("Successfully cached item with key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cache item with key: {Key}", key);
                throw; // Re-throw to allow retry logic in higher layers
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                _logger.LogDebug("Successfully removed cache item with key: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove cache item with key: {Key}", key);
                throw;
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                var server = _redis.GetServer(endpoints.First());
                var keys = new List<RedisKey>();

                await foreach (var key in server.KeysAsync(pattern: $"*{pattern}*"))
                {
                    keys.Add(key);
                }

                if (keys.Count > 0)
                {
                    var db = _redis.GetDatabase();
                    await db.KeyDeleteAsync(keys.ToArray());
                    _logger.LogInformation("Removed {Count} keys matching pattern: {Pattern}", keys.Count, pattern);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to remove keys by pattern: {Pattern}", pattern);
                throw;
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                var server = _redis.GetServer(endpoints.First());

                await server.FlushDatabaseAsync();
                _logger.LogInformation("Cleared entire Redis cache");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear Redis cache");
                throw;
            }
        }
    }
}