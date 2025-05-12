namespace dotnet_redis_demo;

public static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #region Redis Configuration

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "RedisDemo_";
        });

        #endregion
        
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