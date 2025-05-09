using Microsoft.EntityFrameworkCore;
using Redis.Domain.Entities;

namespace Redis.Persistence.Context;

public class ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; } 
    public DbSet<Score> Scores { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>()
            .HasMany(p => p.Scores)
            .WithOne(s => s.Player)
            .HasForeignKey(s => s.PlayerId);
    }
}