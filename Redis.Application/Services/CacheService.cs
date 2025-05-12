using Microsoft.Extensions.Logging;
using Redis.Application.Abstract.Service;
using StackExchange.Redis;

namespace Redis.Application.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly TimeSpan _defaultExpiration;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDatabase database, TimeSpan defaultExpiration, ILogger<CacheService> logger)
    {
        _database = database;
        _defaultExpiration = defaultExpiration;
        _logger = logger;
    }

    public Task SetCacheAsync(string key, object value, TimeSpan? expiration = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var cacheKey = $"{key}";
        var expirationTime = expiration ?? _defaultExpiration;

        try
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            return _database.StringSetAsync(cacheKey, serializedValue, expirationTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting cache for key: {Key}", key);
            throw;
        }
    }

    public async Task<T?> GetCacheAsync<T>(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        try
        {
            var cachedValue = await _database.StringGetAsync(key);
            if (cachedValue.IsNullOrEmpty)
            {
                return default;
            }

            var deserializedValue = System.Text.Json.JsonSerializer.Deserialize<T>(cachedValue!);
            return deserializedValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache for key: {Key}", key);
            throw;
        }
    }

    public Task InvalidateCacheAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        var cacheKey = $"{key}";

        try
        {
            return _database.KeyDeleteAsync(cacheKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating cache for key: {Key}", key);
            throw;
        }
    }
}