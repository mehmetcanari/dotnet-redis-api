using Microsoft.EntityFrameworkCore;
using Redis.Application.Configuration;
using Redis.Persistence.Context;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dotnet_redis_demo;

public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        DiContainer.RegisterServices(builder.Services);
        
        builder.Services.AddControllers();
            
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        DiContainer.RegisterRedis(builder.Services);
        DiContainer.RegisterDatabase(builder.Services);
        
        var app = builder.Build();

        #region Swagger Configuration

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        #endregion
        
        app.MapControllers();
        app.Run();
    }
}