using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Redis.Persistence.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDatabaseContext>
{
    public ApplicationDatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=RedisDemo;Username=postgres;Password=1234",
            b => b.MigrationsAssembly("Redis.Persistence"));

        return new ApplicationDatabaseContext(optionsBuilder.Options);
    }
} 