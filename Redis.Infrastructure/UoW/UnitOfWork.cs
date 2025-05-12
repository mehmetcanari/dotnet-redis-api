using Redis.Infrastructure.Abstract;
using Redis.Persistence.Context;

namespace Redis.Infrastructure.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDatabaseContext _context;

    public UnitOfWork(ApplicationDatabaseContext context)
    {
        _context = context;
    }

    public Task CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}