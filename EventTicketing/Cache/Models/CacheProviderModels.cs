namespace EventTicketing.Cache.Models
{
    public enum CacheLevel
    {
        Local = 1,
        Distributed = 2,
        Persistent = 3
    }

    public interface ICacheProvider
    {
        CacheLevel Level { get; }
        Task<(bool exists, T? value)> TryGetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration);
        Task RemoveAsync(string key);
        Task ClearAsync();
    }
}
