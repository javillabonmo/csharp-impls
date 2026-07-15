namespace WeatherAPI.Services;

using StackExchange.Redis;

using System.Text.Json;

public class RedisCacheService
{
    private readonly IDatabase _db;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _db = redis.GetDatabase();
        _logger = logger;
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        Expiration expiry)
    {
        var json = JsonSerializer.Serialize(value);

        _logger.LogDebug("Setting cache key {Key} with expiry {Expiry}", key, expiry);

        await _db.StringSetAsync(
            key,
            json,
            expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (value.IsNullOrEmpty)
        {
            _logger.LogDebug("Cache key {Key} not found", key);
            return default;
        }
        return JsonSerializer.Deserialize<T>((string)value!);
    }

    public async Task RemoveAsync(string key)
    {
        _logger.LogDebug("Removing cache key {Key}", key);
        await _db.KeyDeleteAsync(key);
    }
}