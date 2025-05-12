using Microsoft.Extensions.Logging;
using Redis.Application.Abstract.Service;
using Redis.Application.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Redis.Application.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly TimeSpan _defaultExpiration;
    private readonly ILogger<CacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public CacheService(IDatabase database, RedisSettings redisSettings, ILogger<CacheService> logger)
    {
        _database = database;
        _defaultExpiration = redisSettings.DefaultExpiration;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            WriteIndented = true
        };
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
            var serializedValue = JsonSerializer.Serialize(value, _jsonOptions);
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

            var deserializedValue = JsonSerializer.Deserialize<T>(cachedValue!, _jsonOptions);
            return deserializedValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cache for key: {Key}", key);
            throw;
        }
    }

    public Task InvalidateCacheAsync(List<string> keys)
    {
        if (keys == null || keys.Count == 0)
        {
            throw new ArgumentNullException(nameof(keys));
        }

        try
        {
            var tasks = keys.Select(key => _database.KeyDeleteAsync(key)).ToArray();
            return Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidating cache for keys: {Keys}", string.Join(", ", keys));
            throw;
        }
    }
}