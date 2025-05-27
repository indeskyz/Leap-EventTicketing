namespace EventTicketing.Cache.Services
{
    public class CacheOptions
    {
        public TimeSpan? Expiration { get; set; }
        public bool BypassLocalCache { get; set; }
    }
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CacheOptions options);
        Task RemoveAsync(string key);
        Task ClearAsync();
    }

}
