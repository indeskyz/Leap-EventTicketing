using EventTicketing.Cache.Models;
using EventTicketing.Cache.Providers;

namespace EventTicketing.Cache.Services
{
    public class CacheService : ICacheService
    {
        private readonly IEnumerable<ICacheProvider> _providers;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IEnumerable<ICacheProvider> providers, ILogger<CacheService> logger)
        {
            _providers = providers.OrderBy(p => p.Level);
            _logger = logger;
        }

        public async Task ClearAsync()
        {
            foreach (var provider in _providers)
            {
                try
                {
                    await provider.ClearAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error clearing {Level} cache", provider.Level);
                }
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheOptions options)
        {
            options ??= new CacheOptions();

            foreach (var provider in _providers.Where(p => !options.BypassLocalCache || p.Level != CacheLevel.Local))
            {
                try
                {
                    var (exists, value) = await provider.TryGetAsync<T>(key);
                    if (exists)
                    {
                        _logger.LogDebug("Cache hit on {Level} for {Key}", provider.Level, key);
                        return value!;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading from {Level} cache", provider.Level);
                }
            }

            var result = await factory();

            _ = Task.Run(async () =>
            {
                foreach (var provider in _providers)
                {
                    try
                    {
                        await provider.SetAsync(key, result, options.Expiration);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error writing to {Level} cache", provider.Level);
                    }
                }
            });

            return result;
        }

        public async Task RemoveAsync(string key)
        {
            foreach (var provider in _providers)
            {
                try
                {
                    await provider.RemoveAsync(key);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing from {Level} cache", provider.Level);
                }
            }
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var redisProvider = _providers.OfType<RedisCacheProvider>().FirstOrDefault();
            if (redisProvider != null)
            {
                try
                {
                    await redisProvider.RemoveByPatternAsync(pattern);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error removing pattern from Redis cache");
                }
            }
        }
    }
}