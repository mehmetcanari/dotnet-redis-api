namespace Redis.Application.Configuration;

public class RedisSettings
{
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(5);
} 