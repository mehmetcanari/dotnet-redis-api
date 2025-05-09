using Redis.Application.Abstract;
using Redis.Persistence.Context;

namespace Redis.Infrastructure.UoW;

public class UnitOfWork(ApplicationDatabaseContext context) : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}