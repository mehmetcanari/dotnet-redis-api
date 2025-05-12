namespace Redis.Application.Abstract.Service;

public interface ICacheService
{
    Task SetCacheAsync(string key, object value, TimeSpan? expiration = null);
    Task<T?> GetCacheAsync<T>(string key);
    Task InvalidateCacheAsync(string key);
}