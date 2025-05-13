using Microsoft.EntityFrameworkCore;
using Redis.Application.Abstract.Service;
using Redis.Application.Configuration;
using Redis.Application.Services;
using Redis.Domain.Entities;
using Redis.Infrastructure.Abstract;
using Redis.Infrastructure.Abstract.Repository;
using Redis.Infrastructure.Repositories;
using Redis.Persistence.Context;
using StackExchange.Redis;

namespace dotnet_redis_demo;

public static class DiContainer
{
    public static void RegisterServices(IServiceCollection services)
    {        
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IGenericRepository<Player>, Repository<Player>>();
        services.AddScoped<IGenericRepository<Score>, Repository<Score>>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IScoreService, ScoreService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IScoreRepository, ScoreRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
    
    public static void RegisterRedis(IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect("localhost:6379"));
            
        services.AddSingleton<IDatabase>(sp =>
        {
            var redis = sp.GetRequiredService<IConnectionMultiplexer>();
            return redis.GetDatabase();
        });
            
        services.AddSingleton<RedisSettings>();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "RedisDemo_";
        });
    }
    
    public static void RegisterDatabase(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDatabaseContext>(options =>
            options.UseNpgsql(
                "Host=localhost;Port=5432;Database=RedisDemo;Username=postgres;Password=1234",
                b => b.MigrationsAssembly("Redis.Persistence")));
    }
}