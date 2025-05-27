using EventTicketing.Cache.Models;
using Microsoft.Extensions.Caching.Memory;

namespace EventTicketing.Cache.Providers
{
    public class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        public CacheLevel Level => CacheLevel.Local;

        public MemoryCacheProvider(IMemoryCache cache)
            => _cache = cache;

        public Task<(bool exists, T? value)> TryGetAsync<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult((value != null, value));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration)
        {
            _cache.Set(key, value, expiration ?? TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _cache.Dispose(); // MemoryCache does not support clear, so we dispose it to reset
            return Task.CompletedTask;
        }
    }
}
